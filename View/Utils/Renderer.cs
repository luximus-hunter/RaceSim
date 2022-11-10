using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace View;

public static class Renderer
{
    private static Direction _direction;

    private const int TileSize = 128;
    private const int CarSize = TileSize / 2;

    #region Graphics

    private const string TrackStraight = @".\Graphics\Scaled\Track\track_straight.png";
    private const string TrackStart = @".\Graphics\Scaled\Track\track_start.png";
    private const string TrackFinish = @".\Graphics\Scaled\Track\track_finish.png";
    private const string TrackCornerLeft = @".\Graphics\Scaled\Track\track_corner_left.png";
    private const string TrackCornerRight = @".\Graphics\Scaled\Track\track_corner_right.png";

    #endregion

    public static BitmapSource DrawTrack(Track track)
    {
        // init the bitmap
        int[] sectionGrid = TrackSimulator.SimulateTrack(track);

        int width = TileSize * sectionGrid[0];
        int height = TileSize * sectionGrid[1];

        Bitmap? bmp = ImageLoader.GenerateBitmap(width, height);
        Graphics graphics = Graphics.FromImage(bmp!);

        // start position to render from
        int x = sectionGrid[2];
        int y = sectionGrid[3];
        _direction = track.StartDirection;

        // draw track sections
        foreach (Section section in track.Sections)
        {
            string imageUrl;
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    imageUrl = TrackStart;
                    break;
                case SectionTypes.Finish:
                    imageUrl = TrackFinish;
                    break;
                case SectionTypes.Straight:
                    imageUrl = TrackStraight;
                    break;
                case SectionTypes.LeftCorner:
                    imageUrl = TrackCornerLeft;
                    break;
                case SectionTypes.RightCorner:
                    imageUrl = TrackCornerRight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Bitmap? trackImage = ImageLoader.GetImageBitmap(imageUrl);
            Graphics trackGraphics = Graphics.FromImage(trackImage!);

            // get section data for current section
            SectionData sectionData = Data.CurrentRace.GetSectionData(section);

            // draw the players, if any
            if (sectionData.Left != null)
            {
                IParticipant player = sectionData.Left;
                Bitmap? playerImage = ImageLoader.GetPlayer(player.TeamColor, section.SectionType);

                if (player.Equipment.IsBroken)
                {
                    playerImage = ImageLoader.GetBrokenPlayer(section.SectionType);
                }

                if (section.SectionType == SectionTypes.LeftCorner)
                {
                    playerImage?.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }

                switch (section.SectionType)
                {
                    case SectionTypes.LeftCorner:
                        trackGraphics.DrawImage(playerImage!, 0, 0, CarSize, CarSize);
                        break;
                    case SectionTypes.RightCorner:
                        trackGraphics.DrawImage(playerImage!, CarSize / 2, CarSize / 2, CarSize, CarSize);
                        break;
                    case SectionTypes.Straight:
                    case SectionTypes.StartGrid:
                    case SectionTypes.Finish:
                    default:
                        trackGraphics.DrawImage(playerImage!, CarSize / 2, 0, CarSize, CarSize);
                        break;
                }
            }

            if (sectionData.Right != null)
            {
                IParticipant player = sectionData.Right;
                Bitmap? playerImage = ImageLoader.GetPlayer(player.TeamColor, section.SectionType);

                if (player.Equipment.IsBroken)
                {
                    playerImage = ImageLoader.GetBrokenPlayer(section.SectionType);
                }

                switch (section.SectionType)
                {
                    case SectionTypes.LeftCorner:
                        playerImage?.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        trackGraphics.DrawImage(playerImage!, CarSize / 2, CarSize / 2, CarSize, CarSize);
                        break;
                    case SectionTypes.RightCorner:
                        trackGraphics.DrawImage(playerImage!, 0, CarSize, CarSize, CarSize);
                        break;
                    case SectionTypes.Straight:
                    case SectionTypes.StartGrid:
                    case SectionTypes.Finish:
                    default:
                        trackGraphics.DrawImage(playerImage!, CarSize / 2, CarSize, CarSize, CarSize);
                        break;
                }
            }

            switch (_direction)
            {
                case Direction.Up:
                    trackImage?.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.Left:
                    trackImage?.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Direction.Down:
                    trackImage?.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.Right:
                    trackImage?.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            graphics.DrawImage(trackImage!, x * TileSize, y * TileSize, TileSize, TileSize);

            // set direction for next track piece
            _direction = TrackSimulator.SimulateSection(section, _direction);

            // adjust the x & y coordinates
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

        // return full track
        return ImageLoader.CreateBitmapSourceFromGdiBitmap(bmp!);
    }
}