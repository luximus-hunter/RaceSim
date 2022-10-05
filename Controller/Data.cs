using Model;
using Spectre.Console;

namespace Controller;

public static class Data
{
    private static Competition Competition { get; set; }
    public static Race CurrentRace { get; private set; }

    public static void Initialize()
    {
        Competition = new Competition();
        AddParticipants();
        AddTracks();
    }

    private static void AddParticipants()
    {
        Competition.Participants.Add(new Driver("Driver A", new Car(100, 10, 10, false), TeamColors.White));
        Competition.Participants.Add(new Driver("Driver B", new Car(100, 10, 10, false), TeamColors.Orange));
        Competition.Participants.Add(new Driver("Driver C", new Car(100, 10, 10, false), TeamColors.Magenta));
        Competition.Participants.Add(new Driver("Driver E", new Car(100, 10, 10, false), TeamColors.Yellow));
        Competition.Participants.Add(new Driver("Driver D", new Car(100, 10, 10, false), TeamColors.Sky));
        // Competition.Participants.Add(new Driver("Driver F", new Car(10, 10, 10, false), TeamColors.Lime));
        // Competition.Participants.Add(new Driver("Driver G", new Car(10, 10, 10, false), TeamColors.Pink));
        // Competition.Participants.Add(new Driver("Driver H", new Car(10, 10, 10, false), TeamColors.Grey));
        // Competition.Participants.Add(new Driver("Driver I", new Car(10, 10, 10, false), TeamColors.Silver));
        // Competition.Participants.Add(new Driver("Driver J", new Car(10, 10, 10, false), TeamColors.Cyan));
        // Competition.Participants.Add(new Driver("Driver K", new Car(10, 10, 10, false), TeamColors.Purple));
        // Competition.Participants.Add(new Driver("Driver L", new Car(10, 10, 10, false), TeamColors.Blue));
        // Competition.Participants.Add(new Driver("Driver M", new Car(10, 10, 10, false), TeamColors.Brown));
        // Competition.Participants.Add(new Driver("Driver N", new Car(10, 10, 10, false), TeamColors.Green));
        // Competition.Participants.Add(new Driver("Driver O", new Car(10, 10, 10, false), TeamColors.Red));
        // Competition.Participants.Add(new Driver("Driver P",  new Car(10, 10, 10, false), TeamColors.Black));
    }

    private static void AddTracks()
    {
        Competition.Tracks.Enqueue(new Track("Track 1", 2, 2, Direction.Left, Color.Green, new[]
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

        Competition.Tracks.Enqueue(new Track("Track 2", 3, 2, Direction.Right, Color.Wheat1, new[]
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
    }

    public static void NextRace()
    {
        Track track = Competition.NextTrack();

        if (track == null)
        {
            throw new Exception("No more tracks left.");
        }

        CurrentRace = new Race(track, Competition.Participants);
    }
}