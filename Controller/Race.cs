using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        public Track Track { get; }
        public List<IParticipant> Participants { get; set; }
        private DateTime StartTime;
        private Random _random;
        public Dictionary<Section, SectionData> Positions { get; set; }
        private Timer _timer;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new Random(DateTime.Now.Millisecond);
            
            Positions = new Dictionary<Section, SectionData>();
            foreach (Section section in track.Sections)
            {
                Positions.Add(section, new SectionData());
            }
            
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
        }

        public SectionData GetSectionData(Section section)
        {
            SectionData value = Positions.GetValueOrDefault(section, null);

            if (value == null)
            {
                value = new SectionData();
                Positions.Add(section, value);
            }

            return value;
        }

        public void RandomizeEquipment()
        {
            for (int i = 0; i < Participants.Count; i++)
            {
                IParticipant participant = Participants[i];
        
                Random r = new Random(DateTime.Now.Millisecond);
                participant.Equipment.Quality = (int)r.NextInt64();
                participant.Equipment.Performance = (int)r.NextInt64();
        
                Participants[i] = participant;
            }
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            foreach (Section section in Track.Sections)
            {
                SectionData sectionData = Positions[section];

                if (sectionData.Left != null)
                {
                    sectionData.DistanceLeft += sectionData.Left.Equipment.Performance * sectionData.Left.Equipment.Speed;
                }
                if (sectionData.Right != null)
                {
                    sectionData.DistanceRight += sectionData.Right.Equipment.Performance * sectionData.Right.Equipment.Speed;
                }
            }
        }

        // public delegate void DriversChanged(object source, DriversChangedEventArgs e);

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public void Start()
        {
            _timer.Start();
        }
    }
}
