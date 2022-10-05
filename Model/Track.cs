using Spectre.Console;

namespace Model;

public class Track
{
    public string Name { get; }
    public LinkedList<Section> Sections { get; }
    public int Padding { get; }
    public Direction StartDirection { get; }
    public Color Background { get; }
    public int Laps { get; }

    public Track(string name, int laps, int padding, Direction startDirection, Color background, IEnumerable<SectionTypes> sections)
    {
        Name = name;
        Padding = padding;
        StartDirection = startDirection;
        Background = background;
        Sections = new LinkedList<Section>();
        Laps = laps;

        foreach (SectionTypes sectionType in sections)
        {
            Sections.AddLast(new Section(sectionType));
        }
    }
}

public enum Direction
{
    Up,
    Left,
    Down,
    Right
}