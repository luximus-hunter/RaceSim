using System;
using System.ComponentModel;
using System.Windows;

namespace View.Windows;

public partial class CompetitionStats
{
    public CompetitionStats()
    {
        InitializeComponent();
    }

    private void OnClosing(object? sender, CancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
        Hide();
    }
}