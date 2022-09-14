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
        #region graphics

        private static readonly string[] _startHorizontal =
        {
            "-----",
            "   @ ",
            "     ",
            " @   ",
            "-----",
        };

        private static readonly string[] _startVertical =
        {
            "|   |",
            "|@  |",
            "|   |",
            "|  @|",
            "|   |",
        };

        private static readonly string[] _finishHorizontal =
        {
            "-----",
            "  #  ",
            "  #  ",
            "  #  ",
            "-----",
        };

        private static readonly string[] _finishVertical =
        {
            "|   |",
            "|   |",
            "|###|",
            "|   |",
            "|   |",
        };

        private static readonly string[] _straightHorizontal =
        {
            "-----",
            "  c  ",
            "     ",
            "  c  ",
            "-----",
        };

        private static readonly string[] _straightVertical =
        {
            "|   |",
            "|   |",
            "|c c|",
            "|   |",
            "|   |",
        };

        private static readonly string[] _cornerLD =
        {
            @"----\",
            "   c|",
            "    |",
            " c  |",
            @"\   |"
        };

        private static readonly string[] _cornerLU =
        {
            "/   |",
            " c  |",
            "    |",
            "   c|",
            "----/",
        };

        private static readonly string[] _cornerRD =
        {
            "/----",
            "|c   ",
            "|    ",
            "|  c ",
            "|   /",
        };

        private static readonly string[] _cornerRU =
        {
            @"|   \",
            "|  c ",
            "|    ",
            "|c   ",
            @"\----",
        };

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

            for (int i = 0; i < tile.Length; i++)
            {
                for (int j = 0; j < tile[i].Length; j++)
                {
                    char c = tile[i][j];
                    Color color;

                    switch (c)
                    {
                        // walls
                        case '-':
                        case '|':
                        case '/':
                        case '\\':
                            color = Color.White;
                            break;
                        // finish line
                        case '#':
                            color = Color.Teal;
                            break;
                        // start position
                        case '@':
                            color = ConsoleColor.DarkGray;
                            break;
                        // road
                        default:
                            color = Color.Black;
                            break;
                    }

                    _canvas.SetPixel(x * _tileSize + j + 1, y * _tileSize + i + 1, color);
                }
            }
        }

        public static void PlaceParticipants()
        {
            foreach (Section section in _currentRace.Track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    // Console.WriteLine("startline");
                    
                    
                }
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