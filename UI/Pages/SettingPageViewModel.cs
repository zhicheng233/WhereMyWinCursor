using System.ComponentModel;
using System.Runtime.CompilerServices;
using WhereMyWinCursor.Core.Config;
using WhereMyWinCursor.Core.Utils;

namespace WhereMyWinCursor.UI.Pages;

public class SettingPageViewModel : INotifyPropertyChanged {
    private readonly MainConfig _config;

    public SettingPageViewModel(MainConfig config) {
        _config = config;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public event Action? Saved;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        ConfigManager.Save(_config);
        Saved?.Invoke();
    }

    // Appearance
    public int ThemeIndex {
        get => _config.UI.Theme.ToLower() switch {
            "dark" => 1,
            "light" => 2,
            _ => 0
        };
        set {
            string newTheme = value switch {
                1 => "dark",
                2 => "light",
                _ => "system"
            };
            if (_config.UI.Theme != newTheme) {
                _config.UI.Theme = newTheme;
                Misc.SetTheme();
                OnPropertyChanged();
            }
        }
    }

    public int LanguageIndex {
        get => _config.UI.Language.ToLower() switch {
            "en_us" => 1,
            "zh_cn" => 2,
            _ => 0
        };
        set {
            string newLang = value switch {
                1 => "en_us",
                2 => "zh_cn",
                _ => "system"
            };
            if (_config.UI.Language != newLang) {
                _config.UI.Language = newLang;
                Misc.SetLanguage();
                OnPropertyChanged();
            }
        }
    }

    // Behavior
    public bool StartWithWindows {
        get => _config.Common.StartWithWindows;
        set {
            if (_config.Common.StartWithWindows != value) {
                _config.Common.StartWithWindows = value;
                Misc.SetStartup(value);
                OnPropertyChanged();
            }
        }
    }

    // Cursor size
    public int InitialSize {
        get => _config.Cursor.CursorSize.InitialSize;
        set {
            if (_config.Cursor.CursorSize.InitialSize != value) {
                _config.Cursor.CursorSize.InitialSize = value;
                OnPropertyChanged();
            }
        }
    }

    public int MaxSize {
        get => _config.Cursor.CursorSize.MaxSize;
        set {
            if (_config.Cursor.CursorSize.MaxSize != value) {
                _config.Cursor.CursorSize.MaxSize = value;
                OnPropertyChanged();
            }
        }
    }

    // Animation
    public int AnimationStep {
        get => _config.Cursor.CursorAnimation.Step;
        set {
            if (_config.Cursor.CursorAnimation.Step != value) {
                _config.Cursor.CursorAnimation.Step = value;
                OnPropertyChanged();
            }
        }
    }

    public int AnimationDelay {
        get => _config.Cursor.CursorAnimation.Delay;
        set {
            if (_config.Cursor.CursorAnimation.Delay != value) {
                _config.Cursor.CursorAnimation.Delay = value;
                OnPropertyChanged();
            }
        }
    }

    public int PauseTime {
        get => _config.Cursor.CursorAnimation.PauseTime;
        set {
            if (_config.Cursor.CursorAnimation.PauseTime != value) {
                _config.Cursor.CursorAnimation.PauseTime = value;
                OnPropertyChanged();
            }
        }
    }
}
