using System.Reflection;

namespace WhereMyWinCursor.Core.Utils;

public static class AppInfo {
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    /// <summary>
    /// Get the application version number (format: v1.0.0)
    /// </summary>
    public static string Version {
        get {
            var version = Assembly.GetName().Version;
            return version != null
                ? $"v{version.Major}.{version.Minor}.{version.Build}"
                : "v1.0.0";
        }
    }

    /// <summary>
    /// Get the full version number (format: 1.0.0.0)
    /// </summary>
    public static string FullVersion {
        get {
            var version = Assembly.GetName().Version;
            return version?.ToString() ?? "1.0.0.0";
        }
    }

    /// <summary>
    /// Get application name
    /// </summary>
    public static string Name =>
        Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "Where My Win Cursor";

    /// <summary>
    /// Get copyright information
    /// </summary>
    public static string Copyright =>
        Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright
        ?? "Copyright © 2025";
}