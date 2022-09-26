using Controller;
using Model;
using Spectre.Console;

namespace ControllerTest
{
    public class Controller_Race_Track
    {
        private Race _r;
        private Track _t;
        private Competition _c;

        [SetUp]
        public void SetUp()
        {
            _c = new Competition();
            _t = new Track("Track Smoll", 5, Direction.Down, Color.Green, new[]
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
            _r = new Race(_t, _c.Participants);
        }

        [Test]
        public void Track_Creation()
        {
            Assert.That(_r.Track == _t);
        }

        [Test]
        public void Track_Secions()
        {
            
        }
    }
}