using System.IO;
using System.Runtime.InteropServices;
using Serilog;
using WhereMyWinCursor.Core.Utils;

namespace WhereMyWinCursor.Core;

public class Cursor {
    //import Windows API
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetSystemCursor(IntPtr hcur, uint id);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType,
        int cxDesired, int cyDesired, uint fuLoad);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr CopyIcon(IntPtr hIcon);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    const uint IMAGE_CURSOR = 2;
    const uint LR_LOADFROMFILE = 0x00000010;

    private Dictionary<uint, string> cursorMap = new Dictionary<uint, string>();

    private void AddCursorIfExists(uint id, string keyName) {
        var path = RegKeyTool.GetCursorThemePath(keyName);
        if (string.IsNullOrWhiteSpace(path)) {
            Log.Warning($"[Cursor] Theme path empty for {keyName}, ID={id}");
            return;
        }

        if (!File.Exists(path)) {
            Log.Warning($"[Cursor] Cursor file not found for {keyName}, ID={id}, Path={path}");
            return;
        }

        cursorMap[id] = path;
    }

    public void ChangCursor(int size, string type) {
        if (cursorMap.Count == 0) return;
        string path = RegKeyTool.GetCursorThemePath(type);
        uint Key = cursorMap.FirstOrDefault(x => x.Value == path).Key;
        var hCur = LoadImage(IntPtr.Zero, path, IMAGE_CURSOR, size, size, LR_LOADFROMFILE);
        if (hCur == IntPtr.Zero) {
            return;
        }

        var hCopy = CopyIcon(hCur);
        DestroyIcon(hCur); // 立即释放原始句柄，防止句柄泄漏

        if (hCopy != IntPtr.Zero) {
            SetSystemCursor(hCopy, Key);
        }
    }

    public static void RecoverCursor() {
        const uint SPI_SETCURSORS = 0x0057;
        SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, 0);
    }


    public void SetCustomCursor() {
        cursorMap = new Dictionary<uint, string>();

        // 根据 Windows OCR_* 常量和注册表键名正确映射
        // OCR_* 常量定义：https://docs.microsoft.com/en-us/windows/win32/menurc/about-cursors

        AddCursorIfExists(32512, "Arrow"); // OCR_NORMAL - 普通选择
        AddCursorIfExists(32513, "IBeam"); // OCR_IBEAM - 文本选择
        AddCursorIfExists(32514, "Wait"); // OCR_WAIT - 等待/忙碌
        AddCursorIfExists(32515, "Cross"); // OCR_CROSS - 十字准星
        AddCursorIfExists(32516, "UpArrow"); // OCR_UP - 向上箭头

        AddCursorIfExists(32645, "SizeNS"); // OCR_SIZENS - 垂直调整
        AddCursorIfExists(32644, "SizeWE"); // OCR_SIZEWE - 水平调整
        AddCursorIfExists(32642, "SizeNWSE"); // OCR_SIZENWSE - 对角调整1
        AddCursorIfExists(32643, "SizeNESW"); // OCR_SIZENESW - 对角调整2
        AddCursorIfExists(32646, "SizeAll"); // OCR_SIZEALL - 全方向移动

        AddCursorIfExists(32648, "No"); // OCR_NO - 禁止/不可用
        AddCursorIfExists(32649, "Hand"); // OCR_HAND - 手型（链接选择）
        AddCursorIfExists(32650, "AppStarting"); // OCR_APPSTARTING - 后台工作
        AddCursorIfExists(32651, "Help"); // OCR_HELP - 帮助选择
        AddCursorIfExists(32631, "NWPen"); // OCR_NWPEN - 手写笔（如果主题支持）
    }
}