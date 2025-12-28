using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace WhereMyWinCursor.UI.Pages;

public partial class SettingPage : Page {
    public SettingPageViewModel? ViewModel { get; }

    public SettingPage() {
        InitializeComponent();
        if (App.config != null) {
            ViewModel = new SettingPageViewModel(App.config);
            DataContext = ViewModel;
            ViewModel.Saved += OnSaved;
        }
    }

    private void OnSaved() {
        var parentWindow = Window.GetWindow(this) as SettingWindows;
        var snackbarPresenter = parentWindow?.SnackbarPresenter;
        if (snackbarPresenter == null) return;

        var snackbar = new Snackbar(snackbarPresenter) {
            Title = "Success",
            Content = "Settings saved successfully",
            Icon = new SymbolIcon(SymbolRegular.Checkmark24),
            Appearance = ControlAppearance.Success,
            Timeout = TimeSpan.FromSeconds(1.5)
        };
        snackbar.Show();
    }
}