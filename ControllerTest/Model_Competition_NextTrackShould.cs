using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
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
            SectionTypes[] t = { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner };
            Track track = new Track("Track 1", t);

            _competition.Tracks.Enqueue(track);

            Track result = _competition.NextTrack();

            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] t = { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner };
            Track track = new Track("Track 1", t);

            _competition.Tracks.Enqueue(track);

            Track result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            SectionTypes[] t = { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner };
            Track track1 = new Track("Track 1", t);
            Track track2 = new Track("Track 2", t);

            _competition.Tracks.Enqueue(track1);
            _competition.Tracks.Enqueue(track2);

            Track result = _competition.NextTrack();

            Assert.AreEqual(result.Name, track1.Name);

            result = _competition.NextTrack();

            Assert.AreEqual(result.Name, track2.Name);
        }
    }
}
