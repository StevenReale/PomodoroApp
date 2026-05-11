using PomodoroApp.Commands;
using PomodoroApp.Models;
using PomodoroApp.Services;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PomodoroApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly PomodoroTimerService _timerService;
    private readonly PomodoroSettings _settings;

    private string _workDurationInput = "25";
    private string _breakDurationInput = "5";

    private SessionMode _currentMode = SessionMode.Work;
    private double _progressPercent;
    private string _startPauseButtonText = "Start";
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

    private void SetSessionMode(SessionMode mode)
    {
        if (_currentMode == mode)
        {
            return;
        }

        _currentMode = mode;
        OnPropertyChanged(nameof(IsWorkSessionSelected));
        OnPropertyChanged(nameof(IsBreakSessionSelected));

        ResetTimerForCurrentSession();
    }

    public bool IsWorkSessionSelected
    {
        get => _currentMode == SessionMode.Work;
        set
        {
            if (value)
            {
                SetSessionMode(SessionMode.Work);
            }
        }
    }

    public bool IsBreakSessionSelected
    {
        get => _currentMode == SessionMode.Break;
        set
        {
            if (value)
            {
                SetSessionMode(SessionMode.Break);
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

    public event EventHandler? SessionCompleted;

    public ICommand StartPauseCommand { get; }
    public ICommand ResetCommand { get; }

    public MainViewModel()
    {
        _settings = new PomodoroSettings();

        _timerService = new PomodoroTimerService();
        _timerService.TimerUpdated += TimerService_TimerUpdated;
        _timerService.SessionCompleted += TimerService_SessionCompleted;

        StartPauseCommand = new RelayCommand(ToggleStartPause);
        ResetCommand = new RelayCommand(ResetTimer);

        ResetTimerForCurrentSession();
        UpdateDisplay();
        UpdateStartPauseButtonText();
    }

    private void TimerService_TimerUpdated(object? sender, EventArgs e)
    {
        UpdateDisplay();
        UpdateStartPauseButtonText();
    }
    
    private void TimerService_SessionCompleted(object? sender, EventArgs e)
    {
        SessionMode completedMode = _currentMode;

        SystemSounds.Asterisk.Play();
        SessionCompleted?.Invoke(this, EventArgs.Empty);
        MessageBox.Show($"{completedMode} session complete.");

        SessionMode nextMode =
            completedMode == SessionMode.Work
                ? SessionMode.Break
                : SessionMode.Work;

        SetSessionMode(nextMode);
    }

    private void ToggleStartPause()
    {
        if (_timerService.IsRunning)
        {
            _timerService.Pause();
        }
        else
        {
            _timerService.Start();
        }
    }

    private void ResetTimer()
    {
        if (!TryApplyDurationSettings())
        {
            return;
        }

        ResetTimerForCurrentSession();
    }

    private void UpdateStartPauseButtonText()
    {
        StartPauseButtonText = _timerService.IsRunning ? "Pause" : "Start";
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

        _settings.WorkDurationMinutes = workMinutes;
        _settings.BreakDurationMinutes = breakMinutes;

        return true;
    }

    private void UpdateDisplay()
    {
        TimerText = _timerService.TimeRemaining.ToString(@"mm\:ss");

        double totalSeconds = _timerService.TotalSessionTime.TotalSeconds;
        if (totalSeconds <= 0)
        {
            ProgressPercent = 0;
            return;
        }

        double percentComplete =
            (totalSeconds - _timerService.TimeRemaining.TotalSeconds)
            / totalSeconds * 100;
        ProgressPercent = percentComplete;
    }

    private void ResetTimerForCurrentSession()
    {
        int minutes = _currentMode == SessionMode.Work
            ? _settings.WorkDurationMinutes
            : _settings.BreakDurationMinutes;

        _timerService.Reset(TimeSpan.FromMinutes(minutes));
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
