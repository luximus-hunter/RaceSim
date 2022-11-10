namespace Model;

public class Section
{
    public static int SectionLength => 100;
    public SectionTypes SectionType { get; }
    public string SectionTypeName => SectionType.ToString();

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