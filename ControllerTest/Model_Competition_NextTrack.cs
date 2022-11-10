// using System.Drawing;
using Spectre.Console;
using Model;

namespace ControllerTest;

[TestFixture]
public class Model_Competition_NextTrack
{
    private Competition _competition;

    [SetUp]
    public void SetUp()
    {
        _competition = new Competition();
    }

    [Test]
    public void NextTrack_EmptyQueue_ReturnNull()
    {
        Track result = _competition.NextTrack();
        Assert.IsNull(result);
    }

    [Test]
    public void NextTrack_OneInQueue_ReturnTrack()
    {
        SectionTypes[] t =
        {
            SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner,
            SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner
        };
        Track track = new("Track 1", 1, 1, Direction.Right, Color.Green, t);

        _competition.Tracks.Enqueue(track);

        Track result = _competition.NextTrack();

        Assert.That(result, Is.EqualTo(track));
    }

    [Test]
    public void NextTrack_OneInQueue_RemoveTrackFromQueue()
    {
        SectionTypes[] t =
        {
            SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner,
            SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner
        };
        Track track = new("Track 1", 1, 1, Direction.Right, Color.Green, t);

        _competition.Tracks.Enqueue(track);

        Track result = _competition.NextTrack();
        result = _competition.NextTrack();

        Assert.IsNull(result);
    }

    [Test]
    public void NextTrack_TwoInQueue_ReturnNextTrack()
    {
        SectionTypes[] t =
        {
            SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner,
            SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner
        };
        Track track1 = new("Track 1", 1, 1, Direction.Right, Color.Green, t);
        Track track2 = new("Track 2", 1, 1, Direction.Right, Color.Green, t);

        _competition.Tracks.Enqueue(track1);
        _competition.Tracks.Enqueue(track2);

        Track result = _competition.NextTrack();

        Assert.That(track1.Name, Is.EqualTo(result.Name));

        result = _competition.NextTrack();

        Assert.That(track2.Name, Is.EqualTo(result.Name));
    }
}