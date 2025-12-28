namespace WhereMyWinCursor.Core.Config;

public class MainConfig {
    public Common.Common Common { get; set; } = new();

    public Cursor.Cursor Cursor { get; set; } = new();

    public UI.UI UI { get; set; } = new();
}