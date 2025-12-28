using Microsoft.Win32;
using Serilog;
using System;
using System.IO;
using System.Collections.Generic;

namespace WhereMyWinCursor.Core.Utils;

public class RegKeyTool {
    private const string CursorRegPath = @"Control Panel\\Cursors"; // registry relative path

    public void RegKeyListener() {
        try {
            SystemEvents.UserPreferenceChanged += OnCursorThemeChanged;
            Log.Information("Registry listener started successfully using SystemEvents");
        }
        catch (Exception ex) {
            Log.Error(ex, "Failed to set up registry listener");
        }
    }

    private void OnCursorThemeChanged(object sender, UserPreferenceChangedEventArgs e) {
        if (e.Category == UserPreferenceCategory.Mouse || e.Category == UserPreferenceCategory.General) {
            Log.Debug($"User preference changed: {e.Category}");
            RefreshCursorTheme();
        }
    }

    private static void RefreshCursorTheme() {
        try {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(CursorRegPath);
            var cursorTheme = key?.GetValue("");
            Log.Debug($"Current Cursor Theme: {cursorTheme}");
        }
        catch (Exception ex) {
            Log.Error(ex, "Failed to refresh cursor theme");
        }
    }

    // Returns raw registry value (may be relative file name, may be absolute path, may be empty string)
    public static string GetCursorThemePath(string keyName) {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(CursorRegPath);
        string keyValue = key?.GetValue(keyName)?.ToString() ?? string.Empty;
        return keyValue;
    }

    // Resolves the cursor file path to an absolute path if possible (Windows\\Cursors fallback). Returns null if not found.
    public static string? GetCursorFileFullPath(string keyName) {
        string raw = GetCursorThemePath(keyName);
        if (string.IsNullOrWhiteSpace(raw)) return null;
        string full = raw;
        if (!Path.IsPathRooted(full)) {
            string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            string candidate = Path.Combine(windir, "Cursors", full);
            if (File.Exists(candidate)) full = candidate;
        }

        return File.Exists(full) ? full : null;
    }

    // Batch helper: returns dictionary of keyName -> full file path (only existing files)
    public static Dictionary<string, string> GetCursorFiles(params string[] keyNames) {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var k in keyNames) {
            var path = GetCursorFileFullPath(k);
            if (path != null) result[k] = path;
        }

        return result;
    }
}