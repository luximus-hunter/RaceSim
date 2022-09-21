using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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
            Competition.Participants.Add(new Driver("Driver A", 10, new Car(10, 10, 10, false), TeamColors.White));
            Competition.Participants.Add(new Driver("Driver B", 10, new Car(10, 10, 10, false), TeamColors.Orange));
            Competition.Participants.Add(new Driver("Driver C", 10, new Car(10, 10, 10, false), TeamColors.Magenta));
            Competition.Participants.Add(new Driver("Driver D", 10, new Car(10, 10, 10, false), TeamColors.Sky));
            Competition.Participants.Add(new Driver("Driver E", 10, new Car(10, 10, 10, false), TeamColors.Yellow));
            Competition.Participants.Add(new Driver("Driver F", 10, new Car(10, 10, 10, false), TeamColors.Lime));
            Competition.Participants.Add(new Driver("Driver G", 10, new Car(10, 10, 10, false), TeamColors.Pink));
            Competition.Participants.Add(new Driver("Driver H", 10, new Car(10, 10, 10, false), TeamColors.Grey));
            Competition.Participants.Add(new Driver("Driver I", 10, new Car(10, 10, 10, false), TeamColors.Silver));
            Competition.Participants.Add(new Driver("Driver J", 10, new Car(10, 10, 10, false), TeamColors.Cyan));
            Competition.Participants.Add(new Driver("Driver K", 10, new Car(10, 10, 10, false), TeamColors.Purple));
            Competition.Participants.Add(new Driver("Driver L", 10, new Car(10, 10, 10, false), TeamColors.Blue));
            Competition.Participants.Add(new Driver("Driver M", 10, new Car(10, 10, 10, false), TeamColors.Brown));
            Competition.Participants.Add(new Driver("Driver N", 10, new Car(10, 10, 10, false), TeamColors.Green));
            Competition.Participants.Add(new Driver("Driver O", 10, new Car(10, 10, 10, false), TeamColors.Red));
            Competition.Participants.Add(new Driver("Driver P", 10, new Car(10, 10, 10, false), TeamColors.Black));
        }

        private static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("Track 1", new[]
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

            Competition.Tracks.Enqueue(new Track("Track 2", new[]
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
            Track nextTrack = Competition.NextTrack() ?? new Track("Track Default", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            });

            CurrentRace = new Race(nextTrack, Competition.Participants);
        }
    }
}