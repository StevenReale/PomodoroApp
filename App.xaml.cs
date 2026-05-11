using PomodoroApp.ViewModels;
using System.Windows;

namespace PomodoroApp
{
    public partial class App : Application
    {
        private MainWindow? _mainWindow;
        private CompactWindow? _compactWindow;
        private MainViewModel? _viewModel;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _viewModel = new MainViewModel();
            _viewModel.SessionCompleted += (_, _) => ShowMainWindow();
            _mainWindow = new MainWindow(_viewModel);
            _mainWindow.Show();
        }

        public void ShowCompactMode()
        {
            _mainWindow?.Hide();
            if (_compactWindow == null)
                _compactWindow = new CompactWindow(_viewModel!);
            _compactWindow.ShowCompact();
        }

        public void ShowMainWindow()
        {
            _compactWindow?.Hide();
            if (_mainWindow != null)
            {
                _mainWindow.Show();
                _mainWindow.Topmost = true;
                _mainWindow.Activate();
                _mainWindow.Topmost = false;
            }
        }
    }
}
