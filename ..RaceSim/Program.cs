using Controller;
using RaceSim;
using System;

namespace RaceSim;

internal static class Program
{
    private static void Main()
    {
        Data.Initialize();
        Data.NextRace();
        Visualisation.Initialize();

        Data.CurrentRace.DriversChanged += Visualisation.DriversChangedEventHandler;
        Data.CurrentRace.RaceEnded += RaceEndedEventHandler;

        while (Data.CurrentRace.Participants.Count > 0)
        {
            for (;;)
            {
                // Visualisation.DrawTrack();
                // Thread.Sleep(500);
            }
        }

        return;
    }

    private static void RaceEndedEventHandler(object sender, EventArgs eventArgs)
    {
        Data.NextRace();
        Data.CurrentRace.DriversChanged += Visualisation.DriversChangedEventHandler;
        Data.CurrentRace.RaceEnded += RaceEndedEventHandler;
        Visualisation.Initialize();
    }
}