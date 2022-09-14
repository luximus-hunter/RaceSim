using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Driver
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Driver_Creation()
        {
            Car c = new Car(10, 20, 30, false);
            Driver d = new Driver("Driver A", 100, c, TeamColors.Red);

            Assert.That(d.Name == "Driver A" && d.Points == 100 && d.Equipment == c && d.TeamColor == TeamColors.Red);
        }

        [Test]
        public void Driver_Modification()
        {
            Car c = new Car(10, 20, 30, false);
            Driver d = new Driver("Driver A", 100, c, TeamColors.Red);

            d.Name = "Driver B";
            d.TeamColor = TeamColors.Blue;

            Assert.That(d.Name == "Driver B" && d.TeamColor == TeamColors.Blue);
        }
    }
}