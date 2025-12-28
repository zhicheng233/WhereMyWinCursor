using System.Reflection;

namespace WhereMyWinCursor.Core.Utils;

public static class AppInfo {
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    /// <summary>
    /// Get the application version number (format: v1.0.0 or v1.0.0-hash)
    /// </summary>
    public static string Version {
        get {
            // 优先读取 InformationalVersion（包含哈希后缀）
            var infoVersion = Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            if (!string.IsNullOrEmpty(infoVersion))
            {
                // 移除 SourceLink 附加的 +hash 部分，保留 -suffix 部分
                var plusIndex = infoVersion.IndexOf('+');
                var version = plusIndex > 0 ? infoVersion[..plusIndex] : infoVersion;
                return $"v{version}";
            }

            // 回退到 AssemblyVersion
            var ver = Assembly.GetName().Version;
            return ver != null
                ? $"v{ver.Major}.{ver.Minor}.{ver.Build}"
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