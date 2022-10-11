using Model;

namespace View;

public static class TrackSimulator
{
    public static int[] SimulateTrack(Track t)
    {
        // start pos
        int x = 3;
        int y = 3;

        // init min max values
        int xMax = x;
        int yMax = y;
        int xMin = x;
        int yMin = y;

        // find out grid size (in tiles)
        foreach (Section section in t.Sections)
        {
            Direction direction = t.StartDirection;

            switch (section.SectionType)
            {
                case SectionTypes.LeftCorner:
                    direction = direction switch
                    {
                        Direction.Up => Direction.Left,
                        Direction.Left => Direction.Down,
                        Direction.Down => Direction.Right,
                        Direction.Right => Direction.Up,
                        _ => direction
                    };

                    break;
                case SectionTypes.RightCorner:
                    direction = direction switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Left => Direction.Up,
                        Direction.Down => Direction.Left,
                        Direction.Right => Direction.Down,
                        _ => direction
                    };

                    break;
            }

            switch (direction)
            {
                case Direction.Up:
                    y--;
                    break;
                case Direction.Left:
                    x--;
                    break;
                case Direction.Down:
                    y++;
                    break;
                case Direction.Right:
                    x++;
                    break;
            }

            if (x > xMax)
            {
                xMax = x;
            }

            if (y > yMax)
            {
                yMax = y;
            }

            if (x < xMin)
            {
                xMin = x;
            }

            if (y < yMin)
            {
                yMin = y;
            }
        }

        int xSize = xMax - xMin + 1;
        int ySize = yMax - yMin + 1;
        
        int xStart = 3 - xMin;
        int yStart = 3 - yMin;

        return new[] { xSize, ySize, xStart, yStart };
    }

    public static Direction SimulateSection(Section s, Direction d)
    {
        switch (s.SectionType)
        {
            case SectionTypes.LeftCorner:
                d = d switch
                {
                    Direction.Up => Direction.Left,
                    Direction.Left => Direction.Down,
                    Direction.Down => Direction.Right,
                    Direction.Right => Direction.Up,
                    _ => d
                };

                break;
            case SectionTypes.RightCorner:
                d = d switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Left => Direction.Up,
                    Direction.Down => Direction.Left,
                    Direction.Right => Direction.Down,
                    _ => d
                };

                break;
        }

        return d;
    }
}