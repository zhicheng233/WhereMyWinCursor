using System.Windows.Controls;
using WhereMyWinCursor.Core.Utils;

namespace WhereMyWinCursor.UI.Pages;

public partial class AboutPage : Page {
    public string AppVersion => AppInfo.Version;
    public string AppName => AppInfo.Name;
    public string Copyright => AppInfo.Copyright;

    public AboutPage() {
        DataContext = this;
        InitializeComponent();
    }
}

