using Controller;
using Model;
using Spectre.Console;

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
            "/.-.-.",
            ".     ",
            "|     ",
            ".  1  ",
            "|   2 ",
            ".    /"
        };

        private static readonly string[] CornerRu = CornerRd.Reverse().ToArray();

        private static readonly string[] CornerLu = new string[CornerRd.Length];
        private static readonly string[] CornerLd = new string[CornerRd.Length];

        #endregion

        private static int _tileSize;
        private static Canvas _canvas;
        private static Direction _direction;
        private static Race _currentRace;

        public static void Initialize(Race race)
        {
            _currentRace = race;
            _direction = _currentRace.Track.StartDirection;

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
                CornerLd[i] = new String(CornerRd[i].ToCharArray().Reverse().ToArray());
                CornerLu[i] = new String(CornerRu[i].ToCharArray().Reverse().ToArray());
            }
        }

        private static bool CheckSquareTiles()
        {
            string[][] tiles = {
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

            // create canvas with correct size, reset direction & set start position
            _canvas = new Canvas(xSize * _tileSize + _currentRace.Track.Padding * 2,
                ySize * _tileSize + _currentRace.Track.Padding * 2);
            _direction = _currentRace.Track.StartDirection;
            x = 3 - xMin;
            y = 3 - yMin;

            // draw background
            for (int i = 0; i < _canvas.Height; i++)
            {
                for (int j = 0; j < _canvas.Width; j++)
                {
                    _canvas.SetPixel(j, i, _currentRace.Track.Background);
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

        private static void SimulateSection(Section section)
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

        private static void DrawSection(Section section, int x, int y)
        {
            string[] tile = new string[_tileSize];

            #region tile selector

            switch (section.SectionType)
            {
                case SectionTypes.Straight:
                    if (_direction == Direction.Up)
                    {
                        tile = StraightUp;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = StraightLeft;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = StraightDown;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = StraightRight;
                    }

                    break;
                case SectionTypes.LeftCorner:
                    if (_direction == Direction.Up)
                    {
                        tile = CornerLd;
                        _direction = Direction.Left;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = CornerRd;
                        _direction = Direction.Down;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = CornerRu;
                        _direction = Direction.Right;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = CornerLu;
                        _direction = Direction.Up;
                    }

                    break;
                case SectionTypes.RightCorner:
                    if (_direction == Direction.Up)
                    {
                        tile = CornerRd;
                        _direction = Direction.Right;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = CornerRu;
                        _direction = Direction.Up;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = CornerLu;
                        _direction = Direction.Left;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = CornerLd;
                        _direction = Direction.Down;
                    }

                    break;
                case SectionTypes.StartGrid:
                    if (_direction == Direction.Up)
                    {
                        tile = StartUp;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = StartLeft;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = StartDown;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = StartRight;
                    }

                    break;
                case SectionTypes.Finish:
                    if (_direction == Direction.Up)
                    {
                        tile = FinishUp;
                    }
                    else if (_direction == Direction.Left)
                    {
                        tile = FinishLeft;
                    }
                    else if (_direction == Direction.Down)
                    {
                        tile = FinishDown;
                    }
                    else if (_direction == Direction.Right)
                    {
                        tile = FinishRight;
                    }

                    break;
            }

            #endregion

            tile = InsertParticipantsInGraphic(tile, _currentRace.Positions[section].Left,
                _currentRace.Positions[section].Right);

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
                        _canvas.SetPixel(x * _tileSize + j + _currentRace.Track.Padding,
                            y * _tileSize + i + _currentRace.Track.Padding, color);
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

            // TODO: throw error when start tiles are not in the beginning
            // TODO: throw error when start tiles are somewhere else

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

        private static string[] InsertParticipantsInGraphic(string[] t, IParticipant p1, IParticipant p2)
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
}