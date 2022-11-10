using System.Drawing;
// using Spectre.Console;

using Model;

namespace Controller;

public static class Data
{
    public static Competition Competition { get; private set; }
    public static Race CurrentRace { get; private set; }

    public static void Initialize()
    {
        Competition = new Competition();
        AddParticipants();
        AddTracks();
    }

    private static void AddParticipants()
    {
        Competition.Participants.Add(new Driver("Driver A", new Car(100, 10, 10, false), TeamColors.Red));
        Competition.Participants.Add(new Driver("Driver B", new Car(100, 10, 10, false), TeamColors.Blue));
        Competition.Participants.Add(new Driver("Driver C", new Car(100, 10, 10, false), TeamColors.Green));
        Competition.Participants.Add(new Driver("Driver E", new Car(100, 10, 10, false), TeamColors.Purple));
        Competition.Participants.Add(new Driver("Driver D", new Car(100, 10, 10, false), TeamColors.Sky));
        Competition.Participants.Add(new Driver("Driver F", new Car(100, 10, 10, false), TeamColors.Yellow));
    }

    private static void AddTracks()
    {
        Competition.Tracks.Enqueue(new Track("Warmup Track", 2, 2, Direction.Right, Color.Green, new[]
        {
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.Finish,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
        }));

        Competition.Tracks.Enqueue(new Track("Bridge Track", 3, 2, Direction.Right, Color.Goldenrod, new[]
        {
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.Finish,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.RightCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner
        }));

        Competition.Tracks.Enqueue(new Track("Baby Park", 8, 5, Direction.Left, Color.Maroon, new[]
        {
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.StartGrid,
            SectionTypes.Finish,
            SectionTypes.RightCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.RightCorner
        }));
    }

    public static void NextRace()
    {
        Track track = Competition.NextTrack();

        if (track == null)
        {
            throw new Exception("No more tracks left.");
        }
        
        CurrentRace?.Stop();

        CurrentRace = new Race(track, Competition.Participants);

        CurrentRace?.PlaceParticipants();
        CurrentRace?.RandomizeEquipment();
    }
}