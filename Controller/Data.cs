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
        public static Competition Competition;
        public static Race CurrentRace; 

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
            SectionTypes[] track1 = { 
                SectionTypes.StartGrid, 
                SectionTypes.Finish, 
                SectionTypes.Straight, 
                SectionTypes.Straight, 
                SectionTypes.RightCorner, 
                SectionTypes.RightCorner, 
                SectionTypes.Straight, 
                SectionTypes.Straight, 
                SectionTypes.Straight, 
                SectionTypes.Straight, 
                SectionTypes.RightCorner, 
                SectionTypes.RightCorner 
            };
            Competition.Tracks.Enqueue(new Track("Track 1", track1));
        }

        public static void NextRace()
        {
            Track nextTrack = Competition.NextTrack();

            if(nextTrack == null)
            {
                SectionTypes[] trackDefault = { 
                    SectionTypes.StartGrid, 
                    SectionTypes.Finish, 
                    SectionTypes.RightCorner, 
                    SectionTypes.RightCorner, 
                    SectionTypes.Straight, 
                    SectionTypes.Straight, 
                    SectionTypes.RightCorner, 
                    SectionTypes.RightCorner 
                };
                nextTrack = new Track("Track Default", trackDefault);
            }

            CurrentRace = new Race(nextTrack, Competition.Participants);
        }
    }
}
