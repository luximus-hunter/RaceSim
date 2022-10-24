using System;
using System.Collections.Generic;
using Model;

namespace View;

public class UpdateScoreboardEventArgs : EventArgs
{
    public string ScoreboardString { get; } = "";

    public UpdateScoreboardEventArgs(List<IParticipant> participants)
    {
        participants.Sort((a, b) => a.Points.CompareTo(b.Points));
        participants.Reverse();
        
        for (int i = 0; i < participants.Count; i++)
        {
            IParticipant p = participants[i];
            ScoreboardString += $"{i + 1} - {p.Name} - {p.Points} points\n";
        }

        ScoreboardString = ScoreboardString.Substring(0, ScoreboardString.Length - 1);
    }
}