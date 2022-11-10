using System.ComponentModel;
using System.Windows;

namespace View.Windows;

public partial class TrackStats
{
    public TrackStats()
    {
        InitializeComponent();
    }

    private void OnClosing(object? sender, CancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
        Hide();
    }
}