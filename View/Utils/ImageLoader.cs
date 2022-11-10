using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Model;

namespace View;

public static class ImageLoader
{
    private static Dictionary<string, Bitmap> _images = new();

    public static Bitmap? GetImageBitmap(string url)
    {
        if (!_images.ContainsKey(url))
        {
            Bitmap bmp = new(url);
            _images.Add(url, bmp);
        }

        return _images[url].Clone() as Bitmap;
    }

    public static void ClearImages()
    {
        _images.Clear();
    }

    public static Bitmap? GenerateBitmap(int width, int height)
    {
        if (!_images.ContainsKey("empty"))
        {
            Bitmap bmp = new(width, height);
            _images.Add("empty", bmp);
        }

        return _images["empty"].Clone() as Bitmap;
    }

    public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
    {
        if (bitmap == null)
        {
            throw new ArgumentNullException("bitmap");
        }

        Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);

        BitmapData bitmapData = bitmap.LockBits(
            rect,
            ImageLockMode.ReadWrite,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        try
        {
            int size = rect.Width * rect.Height * 4;

            return BitmapSource.Create(
                bitmap.Width,
                bitmap.Height,
                bitmap.HorizontalResolution,
                bitmap.VerticalResolution,
                PixelFormats.Bgra32,
                null,
                bitmapData.Scan0,
                size,
                bitmapData.Stride);
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }
    }

    public static Bitmap? GetPlayer(TeamColors tc, SectionTypes st)
    {
        if (st == SectionTypes.LeftCorner || st == SectionTypes.RightCorner)
        {
            return GetImageBitmap(@$".\Graphics\Scaled\Cars\Corner\car_{tc.ToString().ToLower()}.png");
        }

        return GetImageBitmap(@$".\Graphics\Scaled\Cars\Straight\car_{tc.ToString().ToLower()}.png");
    }

    public static Bitmap? GetBrokenPlayer(SectionTypes st)
    {
        if (st == SectionTypes.LeftCorner || st == SectionTypes.RightCorner)
        {
            return GetImageBitmap(@".\Graphics\Scaled\Cars\Corner\car_broken.png");
        }

        return GetImageBitmap(@".\Graphics\Scaled\Cars\Straight\car_broken.png");
    }
}