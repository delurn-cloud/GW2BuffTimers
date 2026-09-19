using GW2TimerCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GW2BuffTimers;

public partial class MainWindow : Window
{
    private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(30);

    private readonly CountdownTimerState _foodTimer = new(BuffDuration);
    private readonly CountdownTimerState _utilityTimer = new(BuffDuration);
    private readonly DispatcherTimer _uiTimer;

    private bool _foodReadySoundPlayed = true;
    private bool _utilityReadySoundPlayed = true;

    public MainWindow()
    {
        InitializeComponent();

        _uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
        _uiTimer.Tick += (_, _) => RefreshTimers();

        Loaded += MainWindow_Loaded;
        Closed += (_, _) => _uiTimer.Stop();
        StateChanged += MainWindow_StateChanged;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        PlaceLowerRight();
        RefreshTimers();
        _uiTimer.Start();
    }

    private void MainWindow_StateChanged(object? sender, EventArgs e)
    {
        if (WindowState == WindowState.Normal)
        {
            PlaceLowerRight();
            Topmost = false;
            Topmost = true;
        }
    }

    private void FoodButton_Click(object sender, RoutedEventArgs e)
    {
        _foodTimer.StartOrReset(DateTime.UtcNow);
        _foodReadySoundPlayed = false;
        RefreshTimers();
    }

    private void UtilityButton_Click(object sender, RoutedEventArgs e)
    {
        _utilityTimer.StartOrReset(DateTime.UtcNow);
        _utilityReadySoundPlayed = false;
        RefreshTimers();
    }

    private void RefreshTimers()
    {
        DateTime now = DateTime.UtcNow;
        bool foodBecameReady = _foodTimer.Tick(now);
        bool utilityBecameReady = _utilityTimer.Tick(now);

        if ((foodBecameReady && !_foodReadySoundPlayed)
            || (utilityBecameReady && !_utilityReadySoundPlayed))
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        if (foodBecameReady)
            _foodReadySoundPlayed = true;
        if (utilityBecameReady)
            _utilityReadySoundPlayed = true;

        RefreshTimerRow(_foodTimer, FoodButton, FoodStatusText, "30m", now);
        RefreshTimerRow(_utilityTimer, UtilityButton, UtilityStatusText, "30m", now);
    }

    private void RefreshTimerRow(
        CountdownTimerState timer,
        Button button,
        TextBlock statusText,
        string idleText,
        DateTime nowUtc)
    {
        bool isRunning = timer.IsRunning;
        bool isReady = timer.IsReady;

        button.Background = (Brush)FindResource(isRunning ? "Brush.Running" : "Brush.Ready");
        button.BorderBrush = (Brush)FindResource(
            isReady ? "Brush.RowBorderReady"
            : isRunning ? "Brush.RowBorderRunning"
            : "Brush.RowBorderIdle");
        button.BorderThickness = isReady || isRunning ? new Thickness(2) : new Thickness(1);
        button.Opacity = 1;
        statusText.Foreground = (Brush)FindResource(isReady ? "Brush.ReadyAccent" : "Brush.Muted");
        statusText.FontWeight = isReady ? FontWeights.Bold : FontWeights.SemiBold;

        if (isRunning)
            statusText.Text = timer.DisplayText(nowUtc);
        else if (isReady)
            statusText.Text = "READY";
        else
            statusText.Text = idleText;
    }

    private void PlaceLowerRight()
    {
        const double margin = 28;
        Rect workArea = SystemParameters.WorkArea;
        Left = Math.Max(0, workArea.Right - Width - margin);
        Top = Math.Max(0, workArea.Bottom - Height - margin);
    }

    private void RootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void HideButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}