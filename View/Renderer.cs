using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace View;

public static class Renderer
{
    private static int _tileSize = 128;
    private static Direction _direction;

    #region Graphics

    private const string _trackStraight = @".\Graphics\Track\track_straight.png";
    private const string _trackStart = @".\Graphics\Track\track_start.png";
    private const string _trackFinish = @".\Graphics\Track\track_finish.png";
    private const string _trackCorner = @".\Graphics\Track\track_corner.png";

    #endregion

    public static BitmapSource DrawTrack(Track track)
    {
        // init the bitmap
        int[] sectionGrid = TrackSimulator.SimulateTrack(track);

        int width = _tileSize * sectionGrid[0];
        int height = _tileSize * sectionGrid[1];

        Bitmap bmp = ImageLoader.GenerateBitmap(width, height);
        Graphics graphics = Graphics.FromImage(bmp);

        // start position to render from
        int x = sectionGrid[2];
        int y = sectionGrid[3];
        _direction = track.StartDirection;

        // color the background
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                bmp.SetPixel(j, i, track.Background);
            }
        }

        // draw track sections
        foreach (Section section in track.Sections)
        {
            string imageUrl = section.SectionType switch
            {
                SectionTypes.StartGrid => _trackStart,
                SectionTypes.Finish => _trackFinish,
                SectionTypes.Straight => _trackStraight,
                SectionTypes.LeftCorner => _trackCorner,
                SectionTypes.RightCorner => _trackCorner
            };

            Bitmap trackSection = ImageLoader.GetImageBitmap(imageUrl);
            
            switch (_direction)
            {
                case Direction.Up:
                    trackSection.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.Left:
                    trackSection.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Direction.Down:
                    trackSection.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.Right:
                    trackSection.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
            }
            
            graphics.DrawImage(trackSection, x*_tileSize, y*_tileSize, _tileSize, _tileSize);

            _direction = TrackSimulator.SimulateSection(section, _direction);
            
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
        return ImageLoader.CreateBitmapSourceFromGdiBitmap(bmp);
    }
}