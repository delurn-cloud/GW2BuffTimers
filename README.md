# GW2 Buff Timers

This is an unofficial fan-made utility for Guild Wars 2. It is not affiliated with,
endorsed by, or associated with ArenaNet, LLC or NCSOFT.

A small always-on-top overlay for Windows that helps you track your Guild Wars 2 food
and utility buffs. It shows two independent 30-minute countdown timers — Food and
Utility — and plays a system sound when a timer reaches "READY" so you know to refresh
your buff.

## Features

- Two manual 30-minute countdown timers: **Food** and **Utility**.
- Tap **Food** or **Utility** to run a fresh 30-minute countdown; tapping a timer again
  (whether it is idle, running, or READY) restarts it from the full 30:00. It counts down
  and shows **READY** when finished.
- Plays a system sound (Asterisk) when a timer becomes ready.
- Compact, always-on-top, borderless overlay anchored to the lower-right of your work area.
- Draggable by its surface; the **X** button minimizes, **Quit** closes the app.
- Single-instance: launching a second copy shows a message and exits.

This app is strictly manual / reference-only — it does not read game memory, inject
input, call any APIs, or automate anything.

## Requirements / Building

- .NET SDK 10 or later (the app targets `net10.0-windows`, WPF; the bundled
  `GW2TimerCore` library targets `net10.0`).
- Build the solution:

  ```sh
  dotnet build GW2BuffTimers.slnx -c Release
  ```

## Repository layout

- `MainWindow.xaml` / `MainWindow.xaml.cs` — primary UI and timer logic.
- `App.xaml` / `App.xaml.cs` — application startup and single-instance handling.
- `GW2TimerCore/` — vendored shared class library providing the `CountdownTimerState`
  countdown logic used by the app.

## License

MIT — see [LICENSE](LICENSE). Copyright (c) 2026 Delurn Cloud.