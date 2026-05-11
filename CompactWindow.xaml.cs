using PomodoroApp.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PomodoroApp;

public partial class CompactWindow : Window
{
    private const double CompactHeight = 72;
    private const double MicroHeight = 12;
    private bool _isMicro;

    public CompactWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        PositionBottomRight();
    }

    public void ShowCompact()
    {
        if (_isMicro)
            SetMicroState(false);
        Show();
        Activate();
    }

    private void PositionBottomRight()
    {
        var workArea = SystemParameters.WorkArea;
        Left = workArea.Right - Width - 12;
        Top = workArea.Bottom - CompactHeight - 12;
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void MicroButton_Click(object sender, RoutedEventArgs e)
    {
        SetMicroState(true);
    }

    private void MicroBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        SetMicroState(false);
    }

    private void MicroExpand_Click(object sender, RoutedEventArgs e)
    {
        SetMicroState(false);
    }

    private void SetMicroState(bool micro)
    {
        _isMicro = micro;
        CompactPanel.Visibility = micro ? Visibility.Collapsed : Visibility.Visible;
        MicroBorder.Visibility = micro ? Visibility.Visible : Visibility.Collapsed;

        double newHeight = micro ? MicroHeight : CompactHeight;
        Top += Height - newHeight;  // keep bottom edge anchored
        Height = newHeight;
    }

    private void RestoreButton_Click(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).ShowMainWindow();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
