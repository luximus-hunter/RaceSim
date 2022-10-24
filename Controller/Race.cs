using Model;
using System.Timers;
using View;
using Timer = System.Timers.Timer;

namespace Controller;

public class Race
{
    public Track Track { get; }

    private readonly List<IParticipant> _participants;
    private readonly Dictionary<Section, SectionData> _positions;
    private readonly Timer _timer;
    private readonly Random _random;
    private readonly Dictionary<IParticipant, int> _roundsCompleted;
    private bool _loadingNextTrack;
    private int _points;

    public event EventHandler<RaceStartedEventArgs> RaceStarted;
    public event EventHandler<DriversChangedEventArgs> DriversChanged;
    public event EventHandler<UpdateScoreboardEventArgs> UpdateScoreboard;
    public event EventHandler<EventArgs> RaceEnded;

    public Race(Track track, List<IParticipant> participants)
    {
        Track = track;
        _participants = participants;
        _points = _participants.Count - 1;

        _timer = new Timer(500);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;

        _random = new Random(DateTime.Now.Millisecond);
        _roundsCompleted = new Dictionary<IParticipant, int>();
        _loadingNextTrack = false;

        _positions = new Dictionary<Section, SectionData>();
        foreach (Section section in track.Sections)
        {
            _positions.Add(section, new SectionData());
        }
    }

    public SectionData GetSectionData(Section section)
    {
        SectionData value = _positions.GetValueOrDefault(section, null);

        if (value == null)
        {
            value = new SectionData();
            _positions.Add(section, value);
        }

        return value;
    }

    public void RandomizeEquipment()
    {
        for (int i = 0; i < _participants.Count; i++)
        {
            IParticipant participant = _participants[i];

            participant.Equipment.Speed = _random.Next(7, 10);
            participant.Equipment.Performance = _random.Next(6, 10);
            participant.Equipment.Quality = _random.Next(50, 100);

            _participants[i] = participant;
        }
    }

    public void PlaceParticipants()
    {
        int startGrids = 0;
        bool startGridsAtStart = false;
        bool startGridsMidTrack = false;
        foreach (Section section in Track.Sections.Reverse())
        {
            if (section.SectionType == SectionTypes.StartGrid)
            {
                startGrids++;
                startGridsAtStart = true;
            }
            else
            {
                if (startGridsAtStart)
                {
                    startGridsMidTrack = true;
                }

                startGridsAtStart = false;
            }
        }

        #region validation

        if (startGrids < 1)
        {
            throw new Exception("No start positions");
        }

        if (!startGridsAtStart)
        {
            throw new Exception("Start positions are not at the start of the track");
        }

        if (startGridsMidTrack)
        {
            throw new Exception("Start positions were found in the middle of the track");
        }

        if (Data.CurrentRace._participants.Count > startGrids * 2)
        {
            throw new Exception("Too many drivers for track");
        }

        #endregion

        int driversToPlace = _participants.Count;
        int driversPlaced = 0;

        for (int index = startGrids - 1; index >= 0; index--)
        {
            if (driversToPlace > 0)
            {
                _positions[Track.Sections.ElementAt(index)].Left = _participants[driversPlaced];
                driversPlaced++;
                driversToPlace--;

                if (driversToPlace > 0)
                {
                    _positions[Track.Sections.ElementAt(index)].Right = _participants[driversPlaced];
                    driversPlaced++;
                    driversToPlace--;
                }
            }
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
            Section nextSection = index + 1 >= Track.Sections.Count
                ? Track.Sections.ElementAt(0)
                : Track.Sections.ElementAt(index + 1);
            SectionData nextSectionData = GetSectionData(nextSection);

            if (sectionData.Left != null)
            {
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
                        if (_roundsCompleted.ContainsKey(sectionData.Left) &&
                            _roundsCompleted[sectionData.Left] == Track.Laps)
                        {
                            sectionData.Left.Points += _points;
                            _points--;

                            sectionData.Left = null;

                            UpdateScoreboard?.Invoke(this, new UpdateScoreboardEventArgs(_participants));
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
                        if (_roundsCompleted.ContainsKey(sectionData.Left) &&
                            _roundsCompleted[sectionData.Left] == Track.Laps)
                        {
                            sectionData.Left.Points += _points;
                            _points--;

                            sectionData.Left = null;

                            UpdateScoreboard?.Invoke(this, new UpdateScoreboardEventArgs(_participants));
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
                        if (_roundsCompleted.ContainsKey(sectionData.Right) &&
                            _roundsCompleted[sectionData.Right] == Track.Laps)
                        {
                            sectionData.Right.Points += _points;
                            _points--;

                            sectionData.Right = null;

                            UpdateScoreboard?.Invoke(this, new UpdateScoreboardEventArgs(_participants));
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
                        if (_roundsCompleted.ContainsKey(sectionData.Right) &&
                            _roundsCompleted[sectionData.Right] == Track.Laps)
                        {
                            sectionData.Right.Points += _points;
                            _points--;

                            sectionData.Right = null;

                            UpdateScoreboard?.Invoke(this, new UpdateScoreboardEventArgs(_participants));
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
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
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

        if (_roundsCompleted.Count != _participants.Count)
        {
            nextTrack = false;
        }

        if (nextTrack && _loadingNextTrack == false)
        {
            _loadingNextTrack = true;
            RaceEnded?.Invoke(this, EventArgs.Empty);
        }
        // TODO: end of ⬆ that garbage
    }

    public void Start()
    {
        RaceStarted?.Invoke(this, new RaceStartedEventArgs(Track));
        UpdateScoreboard?.Invoke(this, new UpdateScoreboardEventArgs(_participants));

        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();

        DriversChanged = null;
    }
}