using Model;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Controller;

public class Race
{
    public Track Track { get; }
    public List<IParticipant> Participants { get; }
    public Dictionary<Section, SectionData> Positions { get; }

    private Timer _timer;
    private readonly Random _random;
    private Dictionary<IParticipant, int> _roundsCompleted;

    public EventHandler<DriversChangedEventArgs> DriversChanged;
    public EventHandler<RaceEndedEventArgs> RaceEnded;

    public Race(Track track, List<IParticipant> participants)
    {
        Track = track;
        Participants = participants;

        _random = new Random(DateTime.Now.Millisecond);
        _roundsCompleted = new Dictionary<IParticipant, int>();
        
        _timer = new Timer(200);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;

        Positions = new Dictionary<Section, SectionData>();
        foreach (Section section in track.Sections)
        {
            Positions.Add(section, new SectionData());
        }
    }

    private SectionData GetSectionData(Section section)
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

            participant.Equipment.Speed = _random.Next(7, 10);
            participant.Equipment.Performance = _random.Next(6, 10);
            participant.Equipment.Quality = _random.Next(50, 100);

            Participants[i] = participant;
        }
    }

    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        bool render = false;

        // loop backwards to do front cars first
        for (int index = Track.Sections.Count - 1; index >= 0; index--)
        {
            // get current and next section
            Section section = Track.Sections.ElementAt(index);
            SectionData sectionData = GetSectionData(section);
            Section nextSection;

            try
            {
                nextSection = Track.Sections.ElementAt(index + 1);
            }
            catch (Exception)
            {
                nextSection = Track.Sections.ElementAt(0);
            }

            SectionData nextSectionData = GetSectionData(nextSection);

            if (sectionData.Left != null)
            {
                // int driverCode = sectionData.Left.GetHashCode();
                
                // add driven distance
                int passedDistance = sectionData.Left.Equipment.Distance();
                sectionData.DistanceLeft += passedDistance;

                // break the car
                if (_random.Next(0, sectionData.Left.Equipment.Quality) == 0)
                {
                    sectionData.Left.Equipment.IsBroken = true;
                    render = true;
                }

                // check if able to go to next section
                if (!sectionData.Left.Equipment.IsBroken && sectionData.DistanceLeft > Section.SectionLength)
                {
                    // try straight ahead
                    if (nextSectionData.Left == null)
                    {
                        // // add points if lapped
                        if (section.SectionType == SectionTypes.Finish)
                        {
                            if (_roundsCompleted.ContainsKey(sectionData.Left))
                            {
                                _roundsCompleted[sectionData.Left]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }
                        
                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Left) && _roundsCompleted[sectionData.Left] == Track.Laps)
                        {
                            sectionData.Left = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Left;
                            nextSectionData.DistanceLeft = sectionData.DistanceLeft - Section.SectionLength;

                            // clear data for current section
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }

                        render = true;
                    }
                    // try diagonal
                    else if (nextSectionData.Right == null)
                    {
                        // add points if lapped
                        if (section.SectionType == SectionTypes.Finish)
                        {
                            if (_roundsCompleted.ContainsKey(sectionData.Left))
                            {
                                _roundsCompleted[sectionData.Left]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }

                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Left) && _roundsCompleted[sectionData.Left] == Track.Laps)
                        {
                            sectionData.Left = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Left;
                            nextSectionData.DistanceRight = sectionData.DistanceLeft - Section.SectionLength;

                            // clear data for current section
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }

                        render = true;
                    }
                }
                else
                {
                    // fix car 5% of the time
                    if (_random.Next(0, 20) == 0)
                    {
                        sectionData.Left.Equipment.IsBroken = false;
                        render = true;
                    }
                }
            }
            else if (sectionData.Right != null)
            {
                // int driverCode = sectionData.Right.GetHashCode();
                
                // add driven distance
                int passedDistance = sectionData.Right.Equipment.Distance();
                sectionData.DistanceRight += passedDistance;

                // break the car
                if (_random.Next(0, sectionData.Right.Equipment.Quality) == 0)
                {
                    sectionData.Right.Equipment.IsBroken = true;
                    render = true;
                }

                // check if able to go to next section
                if (!sectionData.Right.Equipment.IsBroken && sectionData.DistanceRight > Section.SectionLength)
                {
                    // try straight ahead
                    if (nextSectionData.Right == null)
                    {
                        // add points if lapped
                        if (section.SectionType == SectionTypes.Finish)
                        {
                            if (_roundsCompleted.ContainsKey(sectionData.Right))
                            {
                                _roundsCompleted[sectionData.Right]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }
                        
                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Right) && _roundsCompleted[sectionData.Right] == Track.Laps)
                        {
                            sectionData.Right = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Right;
                            nextSectionData.DistanceRight = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;
                        }

                        render = true;
                    }
                    // check diagonal
                    else if (nextSectionData.Left == null)
                    {
                        // add points if lapped
                        if (section.SectionType == SectionTypes.Finish)
                        {
                            if (_roundsCompleted.ContainsKey(sectionData.Right))
                            {
                                _roundsCompleted[sectionData.Right]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }
                        
                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Right) && _roundsCompleted[sectionData.Right] == Track.Laps)
                        {
                            sectionData.Right = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Right;
                            nextSectionData.DistanceLeft = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;
                        }

                        render = true;
                    }
                }
                else
                {
                    // fix car 5% of the time
                    if (_random.Next(0, 20) == 0)
                    {
                        sectionData.Right.Equipment.IsBroken = false;
                        render = true;
                    }
                }
            }
        }

        if (render)
        {
            DriversChanged.Invoke(this, new DriversChangedEventArgs(Track));
        }
        
        // TODO: fix this ⬇ garbage, it works for now...
        bool nextTrack = true;
        
        for (int i = 0; i < Track.Laps; i++)
        {
            if (_roundsCompleted.ContainsValue(i))
            {
                nextTrack = false;
            }
        }

        if (_roundsCompleted.Count != Participants.Count)
        {
            nextTrack = false;
        }

        if (nextTrack)
        {
            RaceEnded.Invoke(this, new RaceEndedEventArgs());
        }
        // TODO: end of ⬆ that garbage
    }

    public void Start()
    {
        _timer.Start();
    }
}