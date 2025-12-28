using System.IO;
using System.Windows;
using Microsoft.Win32;
using Wpf.Ui.Appearance;

namespace WhereMyWinCursor.Core.Utils;

public class Misc {
    public static void SetLanguage() {
        string lang;

        if (App.config.UI.Language.ToUpper() == "SYSTEM") {
            lang = System.Globalization.CultureInfo.InstalledUICulture.Name;
        }
        else {
            lang = App.config.UI.Language;
        }

        // 映射 enum → 文件名
        lang = lang switch {
            "en" => "en-US",
            "zh_cn" => "zh-CN",
            "zh_tw" => "zh-TW",
            _ => lang
        };

        string file = $"Resources/Lang.{lang}.xaml";

        // 移除旧的语言资源
        var oldLang = Application.Current.Resources.MergedDictionaries
            .Where(d => d.Source != null && d.Source.OriginalString.Contains("Lang."))
            .ToList();

        foreach (var d in oldLang)
            Application.Current.Resources.MergedDictionaries.Remove(d);

        // 加载英文作为基础 fallback
        ResourceDictionary fallbackDict = new ResourceDictionary();
        try {
            fallbackDict.Source = new Uri("Resources/Lang.en_US.xaml", UriKind.Relative);
        }
        catch {
            // 英文加载失败则使用空字典
        }

        // 创建最终使用的资源字典
        ResourceDictionary finalDict;

        if (lang != "en-US" && lang != "en_US") {
            // 加载目标语言，并将英文作为其 MergedDictionaries（实现真正的 fallback）
            finalDict = new ResourceDictionary();
            finalDict.MergedDictionaries.Add(fallbackDict); // 先添加英文作为基础

            try {
                ResourceDictionary langDict = new ResourceDictionary();
                langDict.Source = new Uri(file, UriKind.Relative);
                // 将目标语言的每个 key 复制到 finalDict（会覆盖英文）
                foreach (var key in langDict.Keys) {
                    finalDict[key] = langDict[key];
                }
            }
            catch {
                // 目标语言加载失败，finalDict 已有英文 fallback
            }
        }
        else {
            // 目标是英文，直接使用英文字典
            finalDict = fallbackDict;
        }

        Application.Current.Resources.MergedDictionaries.Add(finalDict);
    }

    public static void SetTheme() {
        ApplicationTheme theme;
        if (App.config.UI.Theme.ToUpper() == "SYSTEM") {
            if (IsSystemDarkMode()) {
                theme = ApplicationTheme.Dark;
            }
            else {
                theme = ApplicationTheme.Light;
            }
        }
        else if (App.config.UI.Theme.ToUpper() == "DARK") {
            theme = ApplicationTheme.Dark;
        }
        else {
            theme = ApplicationTheme.Light;
        }

        ApplicationThemeManager.Apply(theme);
    }

    public static bool IsSystemDarkMode() {
        using var key = Registry.CurrentUser.OpenSubKey(
            @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

        var value = key?.GetValue("AppsUseLightTheme");

        return value is int i && i == 0;
    }

    public static void SetStartup(bool enable) {
        string appName = "WhereMyWinCursor";
        string exePath = Environment.ProcessPath!;

        using var key = Registry.CurrentUser
            .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        if (enable) {
            key?.SetValue(appName, exePath);
        }
        else {
            key?.DeleteValue(appName, false);
        }
    }
}