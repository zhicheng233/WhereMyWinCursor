using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Numerics;

namespace WhereMyWinCursor.Core.Utils;

public static class FastCursorDetector {
    static readonly object _initLock = new object();
    static bool _initialized;

    [StructLayout(LayoutKind.Sequential)]
    struct CURSORINFO {
        public int cbSize;
        public int flags;
        public IntPtr hCursor;
        public Point ptScreenPos;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ICONINFO {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    const int CURSOR_SHOWING = 0x00000001;

    [DllImport("user32.dll")]
    static extern bool GetCursorInfo(ref CURSORINFO pci);

    [DllImport("user32.dll")]
    static extern IntPtr CopyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("user32.dll")]
    static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("gdi32.dll", SetLastError = true)]
    static extern bool DeleteObject(IntPtr hObject);

    static readonly int[] CommonCursorIds = {
        32512, 32513, 32514, 32515, 32649, 32648, 32644, 32645, 32646, 32642, 32643, 32650
    };

    static readonly Dictionary<int, string> IdToName = new() {
        { 32512, "Arrow" }, { 32513, "IBeam" }, { 32514, "Wait" }, { 32515, "Crosshair" }, { 32649, "Hand" },
        { 32648, "No" }, { 32644, "SizeWE" }, { 32645, "SizeNS" }, { 32646, "SizeAll" }, { 32642, "SizeNWSE" },
        { 32643, "SizeNESW" }, { 32650, "Help" },
    };

    static readonly Dictionary<string, ulong> KnownHashes = new();
    static readonly Dictionary<IntPtr, string> _systemCursorHandleMap = new(); // cache: handle -> name

    public static void InitializeKnownCursorHashes() {
        if (_initialized) return;
        lock (_initLock) {
            if (_initialized) return;
            foreach (var id in CommonCursorIds) {
                IntPtr h = LoadCursor(IntPtr.Zero, (IntPtr)id);
                if (h != IntPtr.Zero) {
                    // cache handle -> name for fast equality checks later
                    string name = IdToName.TryGetValue(id, out var n) ? n : $"Id_{id}";
                    if (!_systemCursorHandleMap.ContainsKey(h)) _systemCursorHandleMap[h] = name;
                    using var bmp = GetBitmapFromCursorHandle(h);
                    if (bmp != null) {
                        ulong hash = AverageHash(bmp);
                        KnownHashes.TryAdd(name, hash);
                    }
                }
            }

            // Use RegKeyTool helpers to get cursor files from registry
            string[] keys =
                { "Arrow", "IBeam", "Wait", "Crosshair", "Hand", "Help", "NWPen", "No", "SizeNS", "SizeWE", "SizeAll" };
            var files = RegKeyTool.GetCursorFiles(keys);
            foreach (var kv in files) {
                using var bmp = LoadCursorBitmapFromFile(kv.Value);
                if (bmp != null) {
                    ulong h = AverageHash(bmp);
                    KnownHashes.TryAdd(kv.Key, h);
                }
            }

            _initialized = true;
        }
    }

    static IntPtr GetCurrentCursorHandle() {
        CURSORINFO ci = new() { cbSize = Marshal.SizeOf<CURSORINFO>() };
        if (GetCursorInfo(ref ci) && (ci.flags & CURSOR_SHOWING) != 0) return ci.hCursor;
        return IntPtr.Zero;
    }

    /// <summary>
    /// Quickly detect the current cursor type
    /// </summary>
    public static string DetectCursorTypeFast(int hammingThreshold = 10) {
        if (!_initialized) InitializeKnownCursorHashes();
        IntPtr cur = GetCurrentCursorHandle();
        if (cur == IntPtr.Zero) return "Unknown";

        // Fast path: direct handle lookup
        if (_systemCursorHandleMap.TryGetValue(cur, out var directName)) return directName;

        // Fallback: iterate (in case some not in map, e.g., newly added theme loaded after init)
        foreach (var kv in IdToName) {
            IntPtr loaded = LoadCursor(IntPtr.Zero, (IntPtr)kv.Key);
            if (loaded != IntPtr.Zero) {
                if (!_systemCursorHandleMap.ContainsKey(loaded))
                    _systemCursorHandleMap[loaded] = kv.Value; // populate lazily
                if (loaded == cur) return kv.Value;
            }
        }

        using var curBmp = GetBitmapFromCursorHandle(cur);
        if (curBmp == null) return "Unknown";
        ulong curHash = AverageHash(curBmp);
        int bestDist = int.MaxValue;
        string? bestName = null;
        foreach (var kv in KnownHashes) {
            int d = HammingDistance(curHash, kv.Value);
            if (d < bestDist) {
                bestDist = d;
                bestName = kv.Key;
            }
        }

        return bestName != null && bestDist <= hammingThreshold ? bestName : "Custom";
    }

    static Bitmap? GetBitmapFromCursorHandle(IntPtr hCursor) {
        if (hCursor == IntPtr.Zero) return null;
        IntPtr hIcon = CopyIcon(hCursor);
        if (hIcon == IntPtr.Zero) return null;
        if (!GetIconInfo(hIcon, out ICONINFO info)) {
            DestroyIcon(hIcon);
            return null;
        }

        try {
            Bitmap? source = null;
            if (info.hbmColor != IntPtr.Zero) source = Image.FromHbitmap(info.hbmColor);
            else if (info.hbmMask != IntPtr.Zero) source = Image.FromHbitmap(info.hbmMask);
            if (source == null) return null;
            var converted = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(converted)) g.DrawImage(source, 0, 0);
            source.Dispose();
            return converted;
        }
        catch {
            return null;
        }
        finally {
            if (info.hbmColor != IntPtr.Zero) DeleteObject(info.hbmColor);
            if (info.hbmMask != IntPtr.Zero) DeleteObject(info.hbmMask);
            if (hIcon != IntPtr.Zero) DestroyIcon(hIcon);
        }
    }

    static Bitmap? LoadCursorBitmapFromFile(string path) {
        try {
            using Icon ic = new(path);
            using var bmp = ic.ToBitmap();
            var converted = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(converted)) g.DrawImage(bmp, 0, 0);
            return converted;
        }
        catch {
            return null;
        }
    }

    static ulong AverageHash(Bitmap bmp) {
        const int size = 8;
        using Bitmap small = new(size, size, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(small)) {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImage(bmp, 0, 0, size, size);
        }

        var rect = new Rectangle(0, 0, size, size);
        BitmapData data = small.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        try {
            int stride = data.Stride;
            int pixelCount = size * size;
            int sum = 0;
            byte[] luminances = new byte[pixelCount];
            IntPtr scan0 = data.Scan0;
            int idx = 0;
            for (int y = 0; y < size; y++) {
                IntPtr rowPtr = IntPtr.Add(scan0, y * stride);
                for (int x = 0; x < size; x++) {
                    int offset = x * 3;
                    byte b = Marshal.ReadByte(rowPtr, offset);
                    byte g = Marshal.ReadByte(rowPtr, offset + 1);
                    byte r = Marshal.ReadByte(rowPtr, offset + 2);
                    int lum = (r * 299 + g * 587 + b * 114) / 1000;
                    byte l = (byte)lum;
                    luminances[idx++] = l;
                    sum += lum;
                }
            }

            double avg = sum / (double)pixelCount;
            ulong hash = 0UL;
            for (int i = 0; i < luminances.Length; i++)
                if (luminances[i] >= avg)
                    hash |= (1UL << i);
            return hash;
        }
        finally {
            small.UnlockBits(data);
        }
    }

    static int HammingDistance(ulong a, ulong b) => BitOperations.PopCount(a ^ b);
}