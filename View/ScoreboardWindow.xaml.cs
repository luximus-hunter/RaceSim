using System;
using System.Windows.Threading;
using Controller;

namespace View;

public partial class ScoreboardWindow
{
    public ScoreboardWindow()
    {
        InitializeComponent();
    }

    public void UpdateScoreboardEventHandler(object? sender, UpdateScoreboardEventArgs eventArgs)
    {
        ScoreboardText.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() => { ScoreboardText.Text = eventArgs.ScoreboardString; }));
    }
}