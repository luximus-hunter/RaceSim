using Model;

namespace ControllerTest
{
    public class Model_Competition_Participants
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void Participant_Creation()
        {
            Car c = new Car(10, 20, 30, false);
            Driver d = new Driver("Driver A", 100, c, TeamColors.Red);

            _competition.Participants.Add(d);

            Assert.That(_competition.Participants[0] == d);
        }
    }
}