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
            Competition.Participants.Add(new Driver("Driver A", 10, new Car(10, 10, 10, false), TeamColors.Red));
            Competition.Participants.Add(new Driver("Driver B", 10, new Car(10, 10, 10, false), TeamColors.Green));
            Competition.Participants.Add(new Driver("Driver C", 10, new Car(10, 10, 10, false), TeamColors.Blue));
            Competition.Participants.Add(new Driver("Driver D", 10, new Car(10, 10, 10, false), TeamColors.Yellow));
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