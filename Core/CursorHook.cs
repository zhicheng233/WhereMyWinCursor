using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WhereMyWinCursor.Core;

public class CursorHook {
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_MOUSE_LL = 14;
    private static IntPtr _hookId = IntPtr.Zero;
    public static event Action<int, int> OnMouseMove;

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static LowLevelMouseProc _proc = HookCallback;

    public static void Start() {
        _hookId = SetHook(_proc);
    }

    public static void Stop() {
        UnhookWindowsHookEx(_hookId);
    }

    private static IntPtr SetHook(LowLevelMouseProc proc) {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0) {
            var data = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            OnMouseMove?.Invoke(data.pt.x, data.pt.y);
        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }
}