using Serilog;
using WhereMyWinCursor.Core.Config.UI;

namespace WhereMyWinCursor.Core.Config;

public class ConfigChecker {
    private static bool _isAllPass = true;

    private static MainConfig _config = App.config;

    public static bool CheckConfig() {
        _isAllPass = true;
        CheckUIConfig();
        CheckCursorConfig();
        return  _isAllPass;
    }
    private static void CheckUIConfig() {

        if (!Enum.TryParse(_config.UI.Theme, ignoreCase: true, out ThemeEnum theme) ||
            !Enum.IsDefined(typeof(ThemeEnum), theme)) {
            Log.Error("Config:Theme value invalid, Restored to default!");
            _config.UI.Theme = ThemeEnum.System.ToString();
            _isAllPass = false;
        }
    }

    private static void CheckCursorConfig() {

        if (_config.Cursor.CursorSize.InitialSize <= 0) {
            Log.Error("Config:CursorSize.InitialSize value invalid,Restored to default!");
            _config.Cursor.CursorSize.InitialSize = 32;
            _isAllPass = false;
        }

        if (_config.Cursor.CursorSize.MaxSize < _config.Cursor.CursorSize.InitialSize) {
            Log.Error("Config:CursorSize.MaxSize value invalid,Restored to default!");
            _config.Cursor.CursorSize.MaxSize = 128;
            _isAllPass = false;
        }

        if (_config.Cursor.CursorAnimation.Delay < 0) {
            Log.Error("Config:CursorAnimation.Delay value invalid,Restored to default!");
            _config.Cursor.CursorAnimation.Delay = 0;
            _isAllPass = false;
        }

        if (_config.Cursor.CursorAnimation.Step < 1) {
            Log.Error("Config:CursorAnimation.Step value invalid,Restored to default!");
            _config.Cursor.CursorAnimation.Step = 1;
            _isAllPass = false;
        }

        if (_config.Cursor.CursorAnimation.PauseTime < 0) {
            Log.Error("Config:CursorAnimation.WaitTime value invalid,Restored to default!");
            _config.Cursor.CursorAnimation.PauseTime = 1000;
            _isAllPass = false;
        }
    }

}