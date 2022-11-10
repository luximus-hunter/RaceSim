namespace Model;

public interface IParticipant
{
    public string Name { get; }
    public int Points { get; set; }
    public IEquipment Equipment { get; }
    public TeamColors TeamColor { get; }
    public string Color => TeamColor.ToString();
    public int Section { get; set; }

    public void AddPoint();
}

public enum TeamColors
{
    White,
    Orange,
    Magenta,
    Sky,
    Yellow,
    Lime,
    Pink,
    Grey,
    Silver,
    Cyan,
    Purple,
    Blue,
    Brown,
    Green,
    Red,
    Black
}