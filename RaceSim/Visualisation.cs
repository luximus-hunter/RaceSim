using Controller;
using Model;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceSim
{
    public static class Visualisation
    {
        #region event handlers

        public static void DriversChangedEventHandler(object sender, DriversChangedEventArgs e)
        {
            DrawTrack();
        }

        #endregion

        #region graphics old

        // private static readonly string[] _startHorizontal =
        // {
        //     "-----",
        //     "   1 ",
        //     "     ",
        //     " 2   ",
        //     "-----",
        // };
        //
        // private static readonly string[] _startVertical =
        // {
        //     "|   |",
        //     "|1  |",
        //     "|   |",
        //     "|  2|",
        //     "|   |",
        // };
        //
        // private static readonly string[] _finishHorizontal =
        // {
        //     "-----",
        //     "  #1 ",
        //     "  #  ",
        //     " 2#  ",
        //     "-----",
        // };
        //
        // private static readonly string[] _finishVertical =
        // {
        //     "|   |",
        //     "|1  |",
        //     "|###|",
        //     "|  2|",
        //     "|   |",
        // };
        //
        // private static readonly string[] _straightHorizontal =
        // {
        //     "-----",
        //     "  1  ",
        //     "     ",
        //     "  2  ",
        //     "-----",
        // };
        //
        // private static readonly string[] _straightVertical =
        // {
        //     "|   |",
        //     "|   |",
        //     "|1 2|",
        //     "|   |",
        //     "|   |",
        // };
        //
        // private static readonly string[] _cornerLD =
        // {
        //     @"----\",
        //     "    |",
        //     "  1 |",
        //     " 2  |",
        //     @"\   |"
        // };
        //
        // private static readonly string[] _cornerLU =
        // {
        //     "/   |",
        //     " 1  |",
        //     "  2 |",
        //     "    |",
        //     "----/",
        // };
        //
        // private static readonly string[] _cornerRD =
        // {
        //     "/----",
        //     "|    ",
        //     "| 1  ",
        //     "|  2 ",
        //     "|   /",
        // };
        //
        // private static readonly string[] _cornerRU =
        // {
        //     @"|   \",
        //     "|  1 ",
        //     "| 2  ",
        //     "|    ",
        //     @"\----",
        // };

        #endregion

        #region graphics

        private static readonly string[] _startHorizontal =
        {
            "------",
            "      ",
            "    1 ",
            " 2    ",
            "      ",
            "------",
        };

        private static readonly string[] _startVertical =
        {
            "|    |",
            "| 1  |",
            "|    |",
            "|    |",
            "|  2 |",
            "|    |",
        };

        private static readonly string[] _finishHorizontal =
        {
            "as----",
            "sa    ",
            "as  1 ",
            "sa 2  ",
            "as    ",
            "sa----",
        };

        private static readonly string[] _finishVertical =
        {
            "swswsw",
            "wswsws",
            "|    |",
            "| 2  |",
            "|  1 |",
            "|    |",
        };

        private static readonly string[] _straightHorizontal =
        {
            "------",
            "      ",
            "   1  ",
            "  2   ",
            "      ",
            "------",
        };

        private static readonly string[] _straightVertical =
        {
            "|    |",
            "|    |",
            "| 12 |",
            "|    |",
            "|    |",
            "|    |",
        };

        private static readonly string[] _cornerRD =
        {
            "/.-.-.",
            ".     ",
            "|     ",
            ".  1  ",
            "|   2 ",
            ".    /"
        };

        private static readonly string[] _cornerRU = _cornerRD.Reverse().ToArray();

        private static string[] _cornerLU = new string[_cornerRD.Length];
        private static string[] _cornerLD = new string[_cornerRD.Length];

        #endregion

        private static int _tileSize;
        private static Canvas _canvas;
        private static Direction _direction;
        private static Race _currentRace;

        public static void Initialize(Race race)
        {
            _currentRace = race;
            _tileSize = _startHorizontal.Length; // assuming height == width
            _direction = Direction.Right;

            _cornerRD.CopyTo(_cornerLD, 0);
            _cornerRU.CopyTo(_cornerLU, 0);

            for (int i = 0; i < _cornerRD.Length; i++)
            {
                _cornerLD[i] = new String(_cornerRD[i].ToCharArray().Reverse().ToArray());
                _cornerLU[i] = new String(_cornerRU[i].ToCharArray().Reverse().ToArray());
            }

            foreach (var line in _cornerRD)
            {
                Console.WriteLine(line);
            }
            
            foreach (var line in _cornerRU)
            {
                Console.WriteLine(line);
            }

            foreach (var line in _cornerLU)
            {
                Console.WriteLine(line);
            }

            foreach (var line in _cornerLD)
            {
                Console.WriteLine(line);
            }
        }

        public static void DrawTrack()
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
            foreach (Section section in _currentRace.Track.Sections)
            {
                SimulateSection(section, x, y);

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

            // create canvas ft correct size, reset direction & set start position
            _canvas = new Canvas(xSize * _tileSize + 2, ySize * _tileSize + 2);
            _direction = Direction.Right;
            x = 3 - xMin;
            y = 3 - yMin;

            // draw background
            for (int i = 0; i < _canvas.Height; i++)
            {
                for (int j = 0; j < _canvas.Width; j++)
                {
                    int r = new Random().Next();

                    _canvas.SetPixel(j, i, Color.Green);
                }
            }

            // draw all tiles
            foreach (Section section in _currentRace.Track.Sections)
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

            // draw the canvas
            _canvas.Scale = false;
            AnsiConsole.Clear();
            AnsiConsole.Write(_canvas);
        }

        public static void SimulateSection(Section section, int x, int y)
        {
            switch (section.SectionType)
            {
                case SectionTypes.LeftCorner:
                    if (_direction == Direction.Up)
                    {
                        _direction = Direction.Left;
                    }
                    else if (_direction == Direction.Left)
                    {
                        _direction = Direction.Down;
                    }
                    else if (_direction == Direction.Down)
                    {
                        _direction = Direction.Right;
                    }
                    else if (_direction == Direction.Right)
                    {
                        _direction = Direction.Up;
                    }

                    break;
                case SectionTypes.RightCorner:
                    if (_direction == Direction.Up)
                    {
                        _direction = Direction.Right;
                    }
                    else if (_direction == Direction.Left)
                    {
                        _direction = Direction.Up;
                    }
                    else if (_direction == Direction.Down)
                    {
                        _direction = Direction.Left;
                    }
                    else if (_direction == Direction.Right)
                    {
                        _direction = Direction.Down;
                    }

                    break;
            }
        }

        public static void DrawSection(Section section, int x, int y)
        {
            string[] tile = { };

            #region tile selector

            switch (section.SectionType)
            {
                case SectionTypes.Straight:
                    if (_direction == Direction.Left || _direction == Direction.Right)
                    {
                        tile = _straightHorizontal;
                    }
                    else
                    {
                        tile = _straightVertical;
                    }

                    break;
                case SectionTypes.LeftCorner:
                    if (_direction == Direction.Up)
                    {
                        tile = _cornerLD;
                        _direction = Direction.Left;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = _cornerRD;
                        _direction = Direction.Down;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = _cornerRU;
                        _direction = Direction.Right;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = _cornerLU;
                        _direction = Direction.Up;
                    }

                    break;
                case SectionTypes.RightCorner:
                    if (_direction == Direction.Up)
                    {
                        tile = _cornerRD;
                        _direction = Direction.Right;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = _cornerRU;
                        _direction = Direction.Up;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = _cornerLU;
                        _direction = Direction.Left;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = _cornerLD;
                        _direction = Direction.Down;
                    }

                    break;
                case SectionTypes.StartGrid:
                    if (_direction == Direction.Left || _direction == Direction.Right)
                    {
                        tile = _startHorizontal;
                    }
                    else
                    {
                        tile = _startVertical;
                    }

                    break;
                case SectionTypes.Finish:
                    if (_direction == Direction.Left || _direction == Direction.Right)
                    {
                        tile = _finishHorizontal;
                    }
                    else
                    {
                        tile = _finishVertical;
                    }

                    break;
            }

            #endregion

            if (_currentRace.Positions[section].Left != null && _currentRace.Positions[section].Right != null)
            {
                tile = InsertParticipantsInGraphic(tile, _currentRace.Positions[section].Left,
                    _currentRace.Positions[section].Right);
            }
            else if (_currentRace.Positions[section].Left != null)
            {
                tile = InsertParticipantsInGraphic(tile, _currentRace.Positions[section].Left, null);
            }

            for (int i = 0; i < tile.Length; i++)
            {
                for (int j = 0; j < tile[i].Length; j++)
                {
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
                            color = Color.Maroon;
                            break;
                        // finish line
                        case '#':
                            color = Color.Teal;
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
                            color = Color.SkyBlue1;
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
                            color = Color.Grey;
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
                            color = Color.Grey23;
                            break;
                    }

                    #endregion

                    if (c != 'x')
                    {
                        _canvas.SetPixel(x * _tileSize + j + 1, y * _tileSize + i + 1, color);
                    }
                }
            }
        }

        public static void PlaceParticipants()
        {
            int startGrids = 0;
            foreach (Section section in _currentRace.Track.Sections.Reverse())
            {
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    startGrids++;
                }
            }

            if (_currentRace.Participants.Count > startGrids * 2)
            {
                throw new Exception("Too many drivers for track");
            }

            if (startGrids < 1)
            {
                throw new Exception("No start positions");
            }

            // ASSUMING STARTS ARE IN THE BEGINNING

            int driversToPlace = _currentRace.Participants.Count;
            int driversPlaced = 0;

            for (int index = startGrids - 1; index >= 0; index--)
            {
                if (driversToPlace > 0)
                {
                    _currentRace.Positions[_currentRace.Track.Sections.ElementAt(index)].Left =
                        _currentRace.Participants[driversPlaced];
                    driversPlaced++;
                    driversToPlace--;

                    if (driversToPlace > 0)
                    {
                        _currentRace.Positions[_currentRace.Track.Sections.ElementAt(index)].Right =
                            _currentRace.Participants[driversPlaced];
                        driversPlaced++;
                        driversToPlace--;
                    }
                }
            }
        }

        public static string[] InsertParticipantsInGraphic(string[] t, IParticipant p1, IParticipant p2)
        {
            string[] tile = new string[t.Length];

            t.CopyTo(tile, 0);

            for (int i = 0; i < tile.Length; i++)
            {
                string line = tile[i];

                if (p1 != null)
                {
                    line = line.Replace('1', TeamColorToChar(p1.TeamColor));
                }

                if (p2 != null)
                {
                    line = line.Replace('2', TeamColorToChar(p2.TeamColor));
                }

                tile[i] = line;
            }

            return tile;
        }

        public static char TeamColorToChar(TeamColors tc)
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

    enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }
}