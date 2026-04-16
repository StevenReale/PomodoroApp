using PomodoroApp.Commands;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PomodoroApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DispatcherTimer _timer;

    private int _workDurationMinutes = 25;
    private int _breakDurationMinutes = 5;
    private string _workDurationInput = "25";
    private string _breakDurationInput = "5";

    private bool _isWorkSession = true;
    private double _progressPercent;
    private string _startPauseButtonText = "Start";


    private TimeSpan _timeRemaining;
    private TimeSpan _totalSessionTime;

    private string _timerText = "25:00";
    public string TimerText
    {
        get => _timerText;
        set => SetField(ref _timerText, value);
    }

    public string WorkDurationInput
    {
        get => _workDurationInput;
        set => SetField( ref _workDurationInput, value);
    }

    public string BreakDurationInput
    {
        get => _breakDurationInput;
        set => SetField(ref _breakDurationInput, value);
    }

    private void SetSessionMode(bool isWorkSession)
    {
        if (_isWorkSession == isWorkSession)
        {
            return;
        }

        _isWorkSession = isWorkSession;
        OnPropertyChanged(nameof(IsWorkSessionSelected));
        OnPropertyChanged(nameof(IsBreakSessionSelected));

        _timer.Stop();
        ResetTimerForCurrentSession();
        UpdateDisplay();
        UpdateStartPauseButtonText();
    }

    public bool IsWorkSessionSelected
    {
        get => _isWorkSession;
        set
        {
            if (value)
            {
                SetSessionMode(true);
            }
        }
    }

    public bool IsBreakSessionSelected
    {
        get => !_isWorkSession;
        set
        {
            if (value)
            {
                SetSessionMode(false);
            }
        }
    }

    public double ProgressPercent
    {
        get => _progressPercent;
        set => SetField(ref _progressPercent, value);
    }

    public string StartPauseButtonText
    {
        get => _startPauseButtonText;
        set => SetField(ref _startPauseButtonText, value);
    }

    public ICommand StartPauseCommand { get; }
    public ICommand ResetCommand { get; }

    public MainViewModel()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        StartPauseCommand = new RelayCommand(ToggleStartPause);
        ResetCommand = new RelayCommand(ResetTimer);

        ResetTimerForCurrentSession();
        UpdateDisplay();
        UpdateStartPauseButtonText();
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
            UpdateStartPauseButtonText();

            SystemSounds.Asterisk.Play();

            string completedSessionName = _isWorkSession ? "Work" : "Break";
            MessageBox.Show($"{completedSessionName} session complete.");
        }
    }

    private void ToggleStartPause()
    {
        if (_timer.IsEnabled)
        {
            _timer.Stop();
        }
        else
        {
            _timer.Start();
        }

        UpdateStartPauseButtonText();
    }
    private void ResetTimer()
    {
        if (!TryApplyDurationSettings())
        {
            return;
        }

        _timer.Stop();
        ResetTimerForCurrentSession();
        UpdateDisplay();
        UpdateStartPauseButtonText();
    }

    private void UpdateStartPauseButtonText()
    {
        StartPauseButtonText = _timer.IsEnabled ? "Pause" : "Start";
    }

    private bool TryApplyDurationSettings()
    {
        bool workIsValid = int.TryParse(WorkDurationInput, out int workMinutes);
        bool breakIsValid = int.TryParse(BreakDurationInput, out int breakMinutes);

        if (!workIsValid || !breakIsValid || workMinutes <= 0 || breakMinutes <= 0)
        {
            MessageBox.Show("Please enter positive whole numbers for work and break durations.");
            return false;
        }

        _workDurationMinutes = workMinutes;
        _breakDurationMinutes = breakMinutes;

        return true;
    }

    private void UpdateDisplay()
    {
        TimerText = _timeRemaining.ToString(@"mm\:ss");

        double percentComplete =
            (_totalSessionTime.TotalSeconds - _timeRemaining.TotalSeconds)
            / _totalSessionTime.TotalSeconds * 100;
        ProgressPercent = percentComplete;
    }
    private void ResetTimerForCurrentSession()
    {
        int minutes = _isWorkSession ? _workDurationMinutes : _breakDurationMinutes;

        _totalSessionTime = TimeSpan.FromMinutes(minutes);
        _timeRemaining = _totalSessionTime;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
