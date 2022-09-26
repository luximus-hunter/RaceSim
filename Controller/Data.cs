using Model;
using Spectre.Console;

namespace Controller
{
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
            Competition.Participants.Add(new Driver("Driver A", 0, new Car(10, 10, 10, false), TeamColors.White));
            Competition.Participants.Add(new Driver("Driver B", 0, new Car(10, 10, 10, false), TeamColors.Orange));
            Competition.Participants.Add(new Driver("Driver C", 0, new Car(10, 10, 10, false), TeamColors.Magenta));
            Competition.Participants.Add(new Driver("Driver E", 0, new Car(10, 10, 5, false), TeamColors.Yellow));
            Competition.Participants.Add(new Driver("Driver D", 0, new Car(10, 10, 10, false), TeamColors.Sky));
            // Competition.Participants.Add(new Driver("Driver F", 0, new Car(10, 10, 10, false), TeamColors.Lime));
            // Competition.Participants.Add(new Driver("Driver G", 0, new Car(10, 10, 10, false), TeamColors.Pink));
            // Competition.Participants.Add(new Driver("Driver H", 0, new Car(10, 10, 10, false), TeamColors.Grey));
            // Competition.Participants.Add(new Driver("Driver I", 0, new Car(10, 10, 10, false), TeamColors.Silver));
            // Competition.Participants.Add(new Driver("Driver J", 0, new Car(10, 10, 10, false), TeamColors.Cyan));
            // Competition.Participants.Add(new Driver("Driver K", 0, new Car(10, 10, 10, false), TeamColors.Purple));
            // Competition.Participants.Add(new Driver("Driver L", 0, new Car(10, 10, 10, false), TeamColors.Blue));
            // Competition.Participants.Add(new Driver("Driver M", 0, new Car(10, 10, 10, false), TeamColors.Brown));
            // Competition.Participants.Add(new Driver("Driver N", 0, new Car(10, 10, 10, false), TeamColors.Green));
            // Competition.Participants.Add(new Driver("Driver O", 0, new Car(10, 10, 10, false), TeamColors.Red));
            // Competition.Participants.Add(new Driver("Driver P", 0, new Car(10, 10, 10, false), TeamColors.Black));
        }

        private static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("Track 1", 2, Direction.Left, Color.Green, new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,
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

            Competition.Tracks.Enqueue(new Track("Track 2", 2, Direction.Right, Color.Wheat1, new[]
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
}