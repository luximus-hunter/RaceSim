namespace Model;

public class Section
{
    public static int SectionLength { get; } = 100;
    public SectionTypes SectionType { get; private set; }

    public Section(SectionTypes sectionType)
    {
        SectionType = sectionType;
    }

    public override string ToString()
    {
        return SectionType.ToString();
    }
}
    

public enum SectionTypes
{
    Straight,
    LeftCorner,
    RightCorner,
    StartGrid,
    Finish
}