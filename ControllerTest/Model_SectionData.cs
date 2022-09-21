using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_SectionData
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void SectionData_Creation()
        {
            Car c1 = new Car(10, 20, 30, false);
            Car c2 = new Car(10, 20, 30, false);

            Driver d1 = new Driver("Driver A", 100, c1, TeamColors.Red);
            Driver d2 = new Driver("Driver B", 100, c2, TeamColors.Blue);

            SectionData sd = new SectionData();
            sd.Left = d1;
            sd.DistanceLeft = 1;
            sd.Right = d2;
            sd.DistanceRight = 2;

            Assert.That(sd.Left == d1 && sd.DistanceLeft == 1 && sd.Right == d2 && sd.DistanceRight == 2);
        }

        [Test]
        public void SectionData_Modification()
        {
            Car c1 = new Car(10, 20, 30, false);
            Car c2 = new Car(10, 20, 30, false);

            Driver d1 = new Driver("Driver A", 100, c1, TeamColors.Red);
            Driver d2 = new Driver("Driver B", 100, c2, TeamColors.Blue);

            SectionData sd = new SectionData();
            sd.Left = d1;
            sd.DistanceLeft = 1;
            sd.Right = d2;
            sd.DistanceRight = 2;

            Car c3 = new Car(10, 20, 3000, false);
            Driver d3 = new Driver("Driver C", 100, c3, TeamColors.Yellow);

            sd.Left = d3;
            sd.DistanceLeft = 3;

            Assert.That(sd.Left == d3 && sd.DistanceLeft == 3);
        }

        [Test]
        public void SectionType_Create()
        {
            Section s = new Section(SectionTypes.StartGrid);

            Assert.That(s.SectionType == SectionTypes.StartGrid);
        }
    }
}