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
        
        // set start position
        Direction direction = t.StartDirection;

        // find out grid size (in tiles)
        foreach (Section section in t.Sections)
        {
            switch (section.SectionType)
            {
                case SectionTypes.LeftCorner:
                    switch (direction)
                    {
                        case Direction.Up:
                            direction = Direction.Left;
                            break;
                        case Direction.Left:
                            direction = Direction.Down;
                            break;
                        case Direction.Down:
                            direction = Direction.Right;
                            break;
                        case Direction.Right:
                            direction = Direction.Up;
                            break;
                        default:
                            direction = direction;
                            break;
                    }

                    break;
                case SectionTypes.RightCorner:
                    switch (direction)
                    {
                        case Direction.Up:
                            direction = Direction.Right;
                            break;
                        case Direction.Left:
                            direction = Direction.Up;
                            break;
                        case Direction.Down:
                            direction = Direction.Left;
                            break;
                        case Direction.Right:
                            direction = Direction.Down;
                            break;
                        default:
                            direction = direction;
                            break;
                    }

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
                switch (d)
                {
                    case Direction.Up:
                        d = Direction.Left;
                        break;
                    case Direction.Left:
                        d = Direction.Down;
                        break;
                    case Direction.Down:
                        d = Direction.Right;
                        break;
                    case Direction.Right:
                        d = Direction.Up;
                        break;
                }

                break;
            case SectionTypes.RightCorner:
                switch (d)
                {
                    case Direction.Up:
                        d = Direction.Right;
                        break;
                    case Direction.Left:
                        d = Direction.Up;
                        break;
                    case Direction.Down:
                        d = Direction.Left;
                        break;
                    case Direction.Right:
                        d = Direction.Down;
                        break;
                }

                break;
        }

        return d;
    }
}