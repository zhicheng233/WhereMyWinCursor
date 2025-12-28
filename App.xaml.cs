using System.Runtime.InteropServices;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.VisualBasic.CompilerServices;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using WhereMyWinCursor.Core;
using WhereMyWinCursor.Core.Config;
using WhereMyWinCursor.Core.Utils;
using WhereMyWinCursor.UI;

namespace WhereMyWinCursor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public static MainConfig? config;
    public static Cursor cursor;
    public static bool cursorLock = false;
    public static bool cursorKeeper;


    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();

    private TaskbarIcon? _trayIcon;
    private SettingWindows? _settingWindow;

    protected override void OnStartup(StartupEventArgs e) {
        config = ConfigManager.Load();
        // Open Console if DebugMode is true
        if (config.Common.DebugMode){ AllocConsole(); }
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Literate
                )
            .WriteTo.File("logs/log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .CreateLogger();
        Log.Information("Application Starting Up");

        _trayIcon = (TaskbarIcon)FindResource("TrayIcon");

        Misc.SetLanguage();
        Misc.SetTheme();

        InitCursor();
        StartCursorAccelerationMonitor();
        base.OnStartup(e);
    }

    public void Tray_Open_Setting_Click(object sender, RoutedEventArgs routedEventArgs) {
        if (_settingWindow != null) {
            if (!_settingWindow.IsVisible) {
                _settingWindow.Show();
            }
            if (_settingWindow.WindowState == WindowState.Minimized) {
                _settingWindow.WindowState = WindowState.Normal;
            }
            _settingWindow.Activate();
            return;
        }

        _settingWindow = new SettingWindows();
        _settingWindow.Closing += SettingWindow_Closing;
        _settingWindow.Closed += (_, _) => _settingWindow = null;
        _settingWindow.Show();
    }

    private void SettingWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e) {
        e.Cancel = true;
        if (sender is SettingWindows window) {
            window.Hide();
        }
    }

    public void Tray_Exit_Click(object sender, RoutedEventArgs routedEventArgs) {
        if (_settingWindow != null) {
            _settingWindow.Closing -= SettingWindow_Closing;
            _settingWindow.Close();
        }
        base.Shutdown();
    }

    protected override void OnExit(ExitEventArgs e) {
        //Save Config
        ConfigManager.Save(config);
        Log.Information("See Your Next Time!");
        Log.CloseAndFlush();
        base.OnExit(e);
    }

    private void InitCursor() {
        cursor = new Cursor();
        RegKeyTool regKeyTool = new RegKeyTool();
        regKeyTool.RegKeyListener();
    }

    public static async void ChangeCursor() {

        if (cursorLock) {
            Log.Debug("Coursor is locked, skipping animation.");
            return;
        }
        cursorLock = true;
        cursor.SetCustomCursor();

        for (int i = config.Cursor.CursorSize.InitialSize; i <= config.Cursor.CursorSize.MaxSize; i = i + config.Cursor.CursorAnimation.Step) {
            if (config.Cursor.CursorAnimation.Delay==0) {
                await Task.Yield();
            }
            else {
                await Task.Delay(config.Cursor.CursorAnimation.Delay);
            }

            string CursorType = FastCursorDetector.DetectCursorTypeFast();
            cursor.ChangCursor(i, CursorType);
            Log.Debug($"Cursor changed to {i}");
        }

        cursorKeeper = true;
        Task.Run(() => {
            while (true) {

                if (cursorKeeper) {
                    string CursorType = FastCursorDetector.DetectCursorTypeFast();
                    cursor.ChangCursor(config.Cursor.CursorSize.MaxSize, CursorType);
                }
                else {
                    Cursor.RecoverCursor();
                    break;
                }
            }
        });
        await Task.Delay(config.Cursor.CursorAnimation.PauseTime);
        cursorKeeper = false;
        //CD Time
        await Task.Delay(1000);
        cursorLock = false;
    }

    public void StartCursorAccelerationMonitor() {
        DateTime lastTime = DateTime.Now;
        double counter = 0;
        double counterLimit = 4;
        int lastX = 0;
        int x = 0;
        int lastDirection = 0;
        int direction = 0;

        CursorHook.OnMouseMove += (rX, rY) => {
            x = rX;
        };
        CursorHook.Start();

        Task.Run(() => {
            while (true) {
                Thread.Sleep(30);

                int dx = x - lastX;
                lastX = x;
                if (dx > 0) {
                    direction = 1;
                }else if (dx < 0) {
                    direction = -1;
                }
                //Log.Debug("X:"+x.ToString());
                //Log.Debug("位移:"+dx.ToString());
                if (direction != lastDirection && Math.Abs(dx) >=10) {
                    var weight = Math.Abs(dx) * 0.03;
                    counter = 1 + weight ;
                    lastDirection = direction;
                    Log.Debug("dx："+dx.ToString());
                    Log.Debug("weight:" + weight.ToString());
                }

                if (counter >= counterLimit) {
                    ChangeCursor();
                }
            }
        });

        //超过一定时间后重置计数器
        Task.Run(() => {
            while (true) {
                Thread.Sleep(400);
                Log.Debug(counter.ToString());
                counter = 0;
            }
        });
    }
}