namespace Model;

public class Driver : IParticipant
{
    public string Name { get; set; }
    public int Points { get; set; }
    public IEquipment Equipment { get; set; }
    public TeamColors TeamColor { get; set; }
    public int Section { get; set; }

    public Driver(string name, IEquipment equipment, TeamColors teamColor)
    {
        Name = name;
        Points = 0;
        Section = 0;
        Equipment = equipment;
        TeamColor = teamColor;
    }

    public void AddPoint()
    {
        Points++;
    }
}