using System.ComponentModel;
using Model;

namespace Controller;

public class DataContext : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public string TrackName => Data.CurrentRace.Track.Name;

    public IEnumerable<Section> TrackSections => Data.CurrentRace.Track.Sections;

    public IEnumerable<IParticipant> Participants => Data.Competition.GetParticipants.OrderByDescending(p => p.Points);

    public IEnumerable<Track> Tracks =>
        Data.Competition.Tracks.Reverse().Concat(new[] { Data.CurrentRace.Track }).Reverse();

    public DataContext()
    {
        Data.CurrentRace.DriversChanged += OnEvent;
    }

    private void OnEvent(object sender, DriversChangedEventArgs eventArgs)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
}