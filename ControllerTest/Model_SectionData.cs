using Model;

namespace ControllerTest;

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
        Car c1 = new(10, 20, 30, false);
        Car c2 = new(10, 20, 30, false);

        Driver d1 = new("Driver A", c1, TeamColors.Red);
        Driver d2 = new("Driver B", c2, TeamColors.Blue);

        SectionData sd = new()
        {
            Left = d1,
            DistanceLeft = 1,
            Right = d2,
            DistanceRight = 2
        };

        Assert.That(sd.Left == d1 && sd.DistanceLeft == 1 && sd.Right == d2 && sd.DistanceRight == 2);
    }

    [Test]
    public void SectionData_Modification()
    {
        Car c1 = new(10, 20, 30, false);
        Car c2 = new(10, 20, 30, false);

        Driver d1 = new("Driver A", c1, TeamColors.Red);
        Driver d2 = new("Driver B", c2, TeamColors.Blue);

        SectionData sd = new()
        {
            Left = d1,
            DistanceLeft = 1,
            Right = d2,
            DistanceRight = 2
        };

        Car c3 = new(10, 20, 3000, false);
        Driver d3 = new("Driver C", c3, TeamColors.Yellow);

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