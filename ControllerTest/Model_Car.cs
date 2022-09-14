using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Car
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Car_Creation()
        {
            Car c = new Car(10, 20, 30, false);

            Assert.That(c.Quality == 10 && c.Performance == 20 && c.Speed == 30 && c.IsBroken == false);
        }

        [Test]
        public void Car_Modify()
        {
            Car c = new Car(10, 20, 30, false);
            
            c.Performance = 200;
            c.IsBroken = true;

            Assert.That(c.Performance == 200 && c.IsBroken == true);
        }
    }
}