using System;
using System.Drawing;
using Model;

namespace View;

public class RaceStartedEventArgs : EventArgs
{
    public Color Color { get; }
    public string Title { get; }

    public RaceStartedEventArgs(Track track)
    {
        Color = track.Background;
        Title = track.Name;
    }
}