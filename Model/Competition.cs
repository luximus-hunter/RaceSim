﻿namespace Model;

public class Competition
{
    public List<IParticipant> Participants { get; }
    public Queue<Track> Tracks { get; }

    public Competition()
    {
        Participants = new List<IParticipant>();
        Tracks = new Queue<Track>();
    }

    public Track NextTrack()
    {
        if (Tracks.Count > 0)
        {
            return Tracks.Dequeue();
        }

        return null;
    }
}