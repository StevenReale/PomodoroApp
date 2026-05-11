using PomodoroApp.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace PomodoroApp;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void MiniButton_Click(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).ShowCompactMode();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        Application.Current.Shutdown();
    }
}
