using System.Windows;
using WhereMyWinCursor.UI.Pages;

namespace WhereMyWinCursor.UI;

public partial class SettingWindows {
    public SettingWindows() {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) {
        // Navigate to SettingPage by default
        NavigationView.Navigate(typeof(SettingPage));
    }

}