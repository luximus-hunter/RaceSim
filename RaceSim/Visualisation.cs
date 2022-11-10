using Controller;
using Model;
using Spectre.Console;
// using System.Drawing;

namespace RaceSim;

public static class Visualisation
{
    #region event handlers

    public static void DriversChangedEventHandler(object sender, DriversChangedEventArgs e)
    {
        DrawTrack();
    }

    #endregion

    #region graphics

    private static readonly string[] StartRight =
    {
        "------",
        "      ",
        "    1 ",
        " 2    ",
        "      ",
        "------",
    };

    private static readonly string[] StartLeft =
    {
        "------",
        "      ",
        "    2 ",
        " 1    ",
        "      ",
        "------",
    };

    private static readonly string[] StartDown =
    {
        "|    |",
        "| 1  |",
        "|    |",
        "|    |",
        "|  2 |",
        "|    |",
    };

    private static readonly string[] StartUp =
    {
        "|    |",
        "| 2  |",
        "|    |",
        "|    |",
        "|  1 |",
        "|    |",
    };

    private static readonly string[] FinishRight =
    {
        "----as",
        "    sa",
        "   1as",
        " 2  sa",
        "    as",
        "----sa",
    };

    private static readonly string[] FinishLeft =
    {
        "as----",
        "sa    ",
        "as  2 ",
        "sa1   ",
        "as    ",
        "sa----",
    };

    private static readonly string[] FinishDown =
    {
        "swswsw",
        "wswsws",
        "|    |",
        "| 21 |",
        "|    |",
        "|    |",
    };

    private static readonly string[] FinishUp =
    {
        "swswsw",
        "wswsws",
        "|    |",
        "| 21 |",
        "|    |",
        "|    |",
    };

    private static readonly string[] StraightRight =
    {
        "------",
        "      ",
        "    1 ",
        " 2    ",
        "      ",
        "------",
    };

    private static readonly string[] StraightLeft =
    {
        "------",
        "      ",
        "    2 ",
        " 1    ",
        "      ",
        "------",
    };

    private static readonly string[] StraightDown =
    {
        "|    |",
        "| 2  |",
        "|    |",
        "|    |",
        "|  1 |",
        "|    |",
    };

    private static readonly string[] StraightUp =
    {
        "|    |",
        "| 1  |",
        "|    |",
        "|    |",
        "|  2 |",
        "|    |",
    };

    private static readonly string[] CornerRd =
    {
        "......",
        ".     ",
        ".     ",
        ".  1  ",
        ".   2 ",
        ".    /"
    };

    private static readonly string[] CornerRu = CornerRd.Reverse().ToArray();

    private static readonly string[] CornerLu = new string[CornerRd.Length];
    private static readonly string[] CornerLd = new string[CornerRd.Length];

    #endregion

    private static int _tileSize;
    private static Canvas _canvas;
    private static Direction _direction;

    public static void Initialize()
    {
        _direction = Data.CurrentRace.Track.StartDirection;

        if (!CheckSquareTiles())
        {
            throw new Exception("A tile in the tile-set wasn't a square");
        }

        _tileSize = StartLeft.Length;

        // prep corner tiles
        CornerRd.CopyTo(CornerLd, 0);
        CornerRu.CopyTo(CornerLu, 0);

        for (int i = 0; i < CornerRd.Length; i++)
        {
            CornerLd[i] = new string(CornerRd[i].ToCharArray().Reverse().ToArray());
            CornerLu[i] = new string(CornerRu[i].ToCharArray().Reverse().ToArray());
        }
    }

    private static bool CheckSquareTiles()
    {
        string[][] tiles =
        {
            StraightUp, StraightDown, StraightLeft, StraightRight, StartUp, StartDown, StartLeft, StartRight,
            FinishUp, FinishDown, FinishLeft, FinishRight, CornerRd
        };

        foreach (string[] tile in tiles)
        {
            foreach (string line in tile)
            {
                if (tile.Length != line.Length)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static void DrawTrack()
    {
        #region simulate track

        // start pos
        int x = 3;
        int y = 3;

        // init min max values
        int xMax = x;
        int yMax = y;
        int xMin = x;
        int yMin = y;

        // find out grid size (in tiles)
        foreach (Section section in Data.CurrentRace.Track.Sections)
        {
            SimulateSection(section);

            switch (_direction)
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

        #endregion

        #region track

        // create canvas with correct size, reset direction & set start position
        _canvas = new Canvas(xSize * _tileSize + Data.CurrentRace.Track.Padding * 2,
            ySize * _tileSize + Data.CurrentRace.Track.Padding * 2);
        _direction = Data.CurrentRace.Track.StartDirection;
        x = 3 - xMin;
        y = 3 - yMin;

        // draw background
        for (int i = 0; i < _canvas.Height; i++)
        {
            for (int j = 0; j < _canvas.Width; j++)
            {
                 _canvas.SetPixel(j, i, Data.CurrentRace.Track.Background);
            }
        }

        // draw all tiles
        foreach (Section section in Data.CurrentRace.Track.Sections)
        {
            DrawSection(section, x, y);

            switch (_direction)
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
        }

        _canvas.Scale = false;

        #endregion

        #region screen

        Table screen = new();
        screen.RoundedBorder();
        screen.AddColumn($"Track: {Data.CurrentRace.Track.Name} | Laps: {Data.CurrentRace.Track.Laps}");
        screen.AddRow(_canvas);

        #endregion

        // write screen
        AnsiConsole.Clear();
        AnsiConsole.Write(screen);
    }

    private static void SimulateSection(Section section)
    {
        switch (section.SectionType)
        {
            case SectionTypes.LeftCorner:
                _direction = _direction switch
                {
                    Direction.Up => Direction.Left,
                    Direction.Left => Direction.Down,
                    Direction.Down => Direction.Right,
                    Direction.Right => Direction.Up,
                    _ => _direction
                };

                break;
            case SectionTypes.RightCorner:
                _direction = _direction switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Left => Direction.Up,
                    Direction.Down => Direction.Left,
                    Direction.Right => Direction.Down,
                    _ => _direction
                };

                break;
        }
    }

    private static void DrawSection(Section section, int x, int y)
    {
        string[] tile = new string[_tileSize];

         #region tile selector

        switch (section.SectionType)
        {
            case SectionTypes.Straight:
                tile = _direction switch
                {
                    Direction.Up => StraightUp,
                    Direction.Left => StraightLeft,
                    Direction.Down => StraightDown,
                    Direction.Right => StraightRight,
                    _ => tile
                };

                break;
            case SectionTypes.LeftCorner:
                switch (_direction)
                {
                    case Direction.Up:
                        tile = CornerLd;
                        _direction = Direction.Left;
                        break;
                    case Direction.Left:
                        tile = CornerRd;
                        _direction = Direction.Down;
                        break;
                    case Direction.Down:
                        tile = CornerRu;
                        _direction = Direction.Right;
                        break;
                    case Direction.Right:
                        tile = CornerLu;
                        _direction = Direction.Up;
                        break;
                }

                break;
            case SectionTypes.RightCorner:
                switch (_direction)
                {
                    case Direction.Up:
                        tile = CornerRd;
                        _direction = Direction.Right;
                        break;
                    case Direction.Left:
                        tile = CornerRu;
                        _direction = Direction.Up;
                        break;
                    case Direction.Down:
                        tile = CornerLu;
                        _direction = Direction.Left;
                        break;
                    case Direction.Right:
                        tile = CornerLd;
                        _direction = Direction.Down;
                        break;
                }

                break;
            case SectionTypes.StartGrid:
                tile = _direction switch
                {
                    Direction.Up => StartUp,
                    Direction.Left => StartLeft,
                    Direction.Down => StartDown,
                    Direction.Right => StartRight,
                    _ => tile
                };

                break;
            case SectionTypes.Finish:
                tile = _direction switch
                {
                    Direction.Up => FinishUp,
                    Direction.Left => FinishLeft,
                    Direction.Down => FinishDown,
                    Direction.Right => FinishRight,
                    _ => tile
                };

                break;
        }

        #endregion

        tile = InsertParticipantsInGraphic(tile, Data.CurrentRace.GetSectionData(section).Left,
            Data.CurrentRace.GetSectionData(section).Right);

        for (int i = 0; i < tile.Length; i++)
        {
            for (int j = 0; j < tile[i].Length; j++)
            {
                int relX = x * _tileSize + j + Data.CurrentRace.Track.Padding;
                int relY = y * _tileSize + i + Data.CurrentRace.Track.Padding;

                char c = tile[i][j];
                Color color;

                #region color selector

                switch (c)
                {
                    // walls
                    case '-':
                    case '|':
                    case '/':
                    case '\\':
                        color = Color.White;
                        break;
                    case '.':
                        if ((relX + relY) % 2 == 0)
                        {
                            color = Color.Maroon;
                        }
                        else
                        {
                            color = Color.White;
                        }

                        break;
                    // broken driver
                    case '#':
                        color = Color.Grey15;
                        break;
                    // drivers
                    case 'w':
                        color = Color.White;
                        break;
                    case 'o':
                        color = Color.Orange1;
                        break;
                    case 'm':
                        color = Color.Magenta1;
                        break;
                    case 'k':
                        color = Color.LightSkyBlue1;
                        break;
                    case 'y':
                        color = Color.Yellow;
                        break;
                    case 'l':
                        color = Color.Lime;
                        break;
                    case 'p':
                        color = Color.Pink1;
                        break;
                    case 'e':
                        color = Color.Grey23;
                        break;
                    case 's':
                        color = Color.Silver;
                        break;
                    case 'c':
                        color = Color.Cyan1;
                        break;
                    case 'u':
                        color = Color.Purple;
                        break;
                    case 'b':
                        color = Color.Blue;
                        break;
                    case 'n':
                        color = Color.SandyBrown;
                        break;
                    case 'g':
                        color = Color.Green;
                        break;
                    case 'r':
                        color = Color.Red;
                        break;
                    case 'a':
                        color = Color.Black;
                        break;
                    // road
                    default:
                        color = Color.Grey3;
                        break;
                }

                #endregion

                if (c != 'x')
                {
                    _canvas.SetPixel(relX, relY, color);
                }
            }
        }
    }

    private static string[] InsertParticipantsInGraphic(string[] t, IParticipant p1, IParticipant p2)
    {
        string[] tile = new string[t.Length];

        t.CopyTo(tile, 0);

        for (int i = 0; i < tile.Length; i++)
        {
            string line = tile[i];

            if (p1 != null)
            {
                if (!p1.Equipment.IsBroken)
                {
                    line = line.Replace('1', TeamColorToChar(p1.TeamColor));
                }
                else
                {
                    line = line.Replace('1', '#');
                }
            }

            if (p2 != null)
            {
                if (!p2.Equipment.IsBroken)
                {
                    line = line.Replace('2', TeamColorToChar(p2.TeamColor));
                }
                else
                {
                    line = line.Replace('2', '#');
                }
            }

            tile[i] = line;
        }

        return tile;
    }

    private static char TeamColorToChar(TeamColors tc)
    {
        switch (tc)
        {
            case TeamColors.White:
                return 'w';
            case TeamColors.Orange:
                return 'o';
            case TeamColors.Magenta:
                return 'm';
            case TeamColors.Sky:
                return 'k';
            case TeamColors.Yellow:
                return 'y';
            case TeamColors.Lime:
                return 'l';
            case TeamColors.Pink:
                return 'p';
            case TeamColors.Grey:
                return 'e';
            case TeamColors.Silver:
                return 's';
            case TeamColors.Cyan:
                return 'c';
            case TeamColors.Purple:
                return 'u';
            case TeamColors.Blue:
                return 'b';
            case TeamColors.Brown:
                return 'n';
            case TeamColors.Green:
                return 'g';
            case TeamColors.Red:
                return 'r';
            case TeamColors.Black:
                return 'a';
            default:
                return ' ';
        }
    }
}