# PomodoroApp

A simple desktop Pomodoro timer built in C# with WPF.

## Goal

Build a complete start-to-finish desktop project with a real UI, working timer behavior, and a publishable Windows executable.

We are intentionally building this in a pedagogical order:

1. Make it work in the simplest honest way
2. Improve behavior
3. Improve structure
4. Publish it

## Tech Stack

- C#
- WPF
- .NET 10

## Version 1 Roadmap

### 1. Create the WPF project
- Create a WPF app in C#
- Target .NET 10
- Confirm the blank app runs

Status: Done

### 2. Build the basic UI skeleton
- Add app title
- Add session label
- Add large timer display
- Add progress bar
- Add Start / Pause / Reset buttons
- Add work and break duration inputs

Status: Done

### 3. Wire up the timer
- Create a `DispatcherTimer`
- Hook up the `Tick` event
- Make Start work
- Make Pause work
- Make Reset work
- Update timer text and progress bar each second

Status: Done

### 4. Wire up the settings inputs
- Name the duration text boxes
- Read work and break durations from the UI
- Validate input
- Apply new values on Reset

Status: Done

### 5. Use explicit mode selection in the UI
- Let the user explicitly choose Work or Break mode
- Load the selected mode's duration on Reset
- Stop and notify when a session completes
- Do not auto-switch modes

Status: Done

### 6. Polish basic behavior
- Replace separate Start and Pause buttons with a single toggle button
- Prevent invalid input from crashing the app
- Add a simple completion notification with a beep and message
- Decide on any final v1 UI and interaction tweaks

Status: In progress

### 7. Refactor into light structure
- Move app behavior out of `MainWindow.xaml.cs`
- Introduce a `MainViewModel`
- Introduce a `RelayCommand`
- Bind UI controls to ViewModel properties and commands
- Extract timer logic into a `PomodoroTimerService`
- Add a `PomodoroSettings` model if it improves clarity

Status: In progress

### 8. Publish the executable
- Build a release version
- Publish a Windows executable
- Test the published app outside Visual Studio

Status: Planned

## Version 2 Roadmap

### 1. Session counting
- Count completed work sessions
- Optionally show daily completed pomodoros

### 2. Auto-start options
- Auto-start breaks
- Auto-start work sessions

### 3. Long break logic
- Use a longer break after every N work sessions
- Make the long break duration configurable

### 4. Settings persistence
- Save settings to local JSON
- Restore settings on launch

### 5. Notifications
- Add a completion sound
- Add Windows notifications if desired

### 6. Window behavior improvements
- Always on top
- Minimize to tray
- Tray icon with quick controls

### 7. UI polish
- Improve layout and spacing
- Add styling
- Optionally add a circular progress display

### 8. Daily stats and history
- Track completed sessions by day
- Add a lightweight stats view

### 9. Distribution improvements
- Make the app easier to install and share
- Optionally create an installer

## Current Status

Completed so far:
- Project created
- UI skeleton created
- Basic timer wiring completed
- Duration settings apply through Reset
- Explicit Work / Break mode selection added
- Minor UI polish applied to mode selector
- Replace separate Start and Pause buttons with a single toggle button
- Polish completion behavior and button-state behavior

Immediate next step:
- Refactor the app to use a `MainViewModel` and bindings

Step after that:
- Extract timer behavior into a `PomodoroTimerService`

## Suggested Repository Structure

```text
PomodoroApp/
  README.md
  App.xaml
  App.xaml.cs
  MainWindow.xaml
  MainWindow.xaml.cs
```

Later, after refactoring:

```text
PomodoroApp/
  README.md
  App.xaml
  App.xaml.cs
  MainWindow.xaml
  MainWindow.xaml.cs
  ViewModels/
    MainViewModel.cs
  Services/
    PomodoroTimerService.cs
  Models/
    PomodoroSettings.cs
  Commands/
    RelayCommand.cs
```

## Notes for Ourselves

We are not trying to overengineer v1. The goal is a working, understandable desktop app first. Clean architecture comes after the core behavior works.
