namespace WhereMyWinCursor.Core.Config.Cursor;

public class CursorAnimation {
    public int Step { get; set; } = 1;
    // ms, use Yield() when Delay value = 0
    public int Delay { get; set; } = 0;
    // ms
    public int PauseTime { get; set; } = 1000;
}