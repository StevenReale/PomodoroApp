using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PomodoroApp;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer;

    private int _workDurationMinutes = 25;
    private int _breakDurationMinutes = 5;

    private bool _isWorkSession = true;
    private TimeSpan _timeRemaining;
    private TimeSpan _totalSessionTime;

    public MainWindow()
    {
        InitializeComponent();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        ResetTimerForCurrentSession();
        UpdateDisplay();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_timeRemaining.TotalSeconds > 0)
        {
            _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            UpdateDisplay();
        }
        else
        {
            _timer.Stop();
            MessageBox.Show("Session complete.");
        }
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        _timer.Start();
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {  
        _timer.Stop();
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        ResetTimerForCurrentSession();
        UpdateDisplay();
    }

    private void ResetTimerForCurrentSession()
    {
        int minutes = _isWorkSession ? _workDurationMinutes : _breakDurationMinutes;

        _totalSessionTime = TimeSpan.FromMinutes(minutes);
        _timeRemaining = _totalSessionTime;
    }

    private void UpdateDisplay()
    {
        SessionLabel.Text = _isWorkSession ? "Work Session" : "Break Session";
        TimerText.Text = _timeRemaining.ToString(@"mm\:ss");

        double percentComplete =
            (_totalSessionTime.TotalSeconds - _timeRemaining.TotalSeconds)
            / _totalSessionTime.TotalSeconds * 100;
        TimerProgressBar.Value = percentComplete;
    }

}