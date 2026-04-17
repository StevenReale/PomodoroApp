using System.Windows.Threading;

namespace PomodoroApp.Services;

public class PomodoroTimerService
{
    private readonly DispatcherTimer _timer;

    private TimeSpan _timeRemaining;
    private TimeSpan _totalSessionTime;

    public bool IsRunning => _timer.IsEnabled;
    public TimeSpan TimeRemaining => _timeRemaining;
    public TimeSpan TotalSessionTime => _totalSessionTime;

    public event EventHandler? TimerUpdated;
    public event EventHandler? SessionCompleted;

    public PomodoroTimerService()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnTimerTick;
    }

    public void Start()
    {
        _timer.Start();
        OnTimerUpdated();
    }

    public void Pause()
    {
        _timer.Stop();
        OnTimerUpdated();
    }

    public void Reset(TimeSpan duration)
    {
        _timer.Stop();
        _totalSessionTime = duration;
        _timeRemaining = duration;
        OnTimerUpdated();
    }
    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (_timeRemaining.TotalSeconds > 0)
        {
            _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            OnTimerUpdated();
        }
        else
        {
            _timer.Stop();
            OnTimerUpdated();
            SessionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTimerUpdated()
    {
        TimerUpdated?.Invoke(this, EventArgs.Empty);
    }
}
