using Model;
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
        private readonly Timer _timer;

        public EventHandler<DriversChangedEventArgs> DriversChanged;

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

            _timer = new Timer(200);
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

                Random r = new(DateTime.Now.Millisecond);
                participant.Equipment.Quality = (int)r.NextInt64();
                participant.Equipment.Performance = (int)r.NextInt64();

                Participants[i] = participant;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //              _
            //  _ __   __ _(_)_ __
            // | '_ \ / _` | | '_ \
            // | |_) | (_| | | | | |
            // | .__/ \__,_|_|_| |_|
            // |_|
            
            bool render = false;

            // loop backwards to do front cars first
            for (int index = Track.Sections.Count - 1; index >= 0; index--)
            {
                Section section = Track.Sections.ElementAt(index);
                SectionData sectionData = GetSectionData(section);
                Section nextSection;

                try
                {
                    nextSection = Track.Sections.ElementAt(index + 1);
                }
                catch (Exception ex)
                {
                    nextSection = Track.Sections.ElementAt(0);
                }

                SectionData nextSectionData = GetSectionData(nextSection);
                
                if (sectionData.Left != null)
                {
                    int passedDistance = sectionData.Left.Equipment.Distance();

                    if (sectionData.DistanceLeft + passedDistance > Section.SectionLength)
                    {
                        if (nextSectionData.Left == null)
                        {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Left;
                            nextSectionData.DistanceLeft = sectionData.DistanceLeft - Section.SectionLength;

                            // clear data for current section
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;

                            render = true;
                        }
                        else if (nextSectionData.Right == null)
                        {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Left;
                            nextSectionData.DistanceRight = sectionData.DistanceLeft - Section.SectionLength;

                            // clear data for current section
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;

                            render = true;
                        }
                        else
                        {
                            Console.WriteLine($"{sectionData.Left.TeamColor} wil er langs");
                            sectionData.DistanceLeft = Section.SectionLength;
                        }
                    }
                    else
                    {
                        sectionData.DistanceLeft += passedDistance;
                    }
                }
                else if (sectionData.Right != null)
                {
                    int passedDistance = sectionData.Right.Equipment.Distance();

                    if (sectionData.DistanceRight + passedDistance > Section.SectionLength)
                    {
                        if (nextSectionData.Right == null)
                        {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Right;
                            nextSectionData.DistanceRight = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;

                            render = true;
                        }
                        else if (nextSectionData.Left == null)
                        {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Right;
                            nextSectionData.DistanceLeft = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;

                            render = true;
                        }
                        else
                        {
                            Console.WriteLine($"{sectionData.Right.TeamColor} wil er langs");
                            sectionData.DistanceRight = Section.SectionLength;
                        }
                    }
                    else
                    {
                        sectionData.DistanceRight += passedDistance;
                    }
                }
            }

            if (render)
            {
                DriversChanged.Invoke(this, new DriversChangedEventArgs(Track));
            }
        }

        public void Start()
        {
            _timer.Start();
        }
    }
}