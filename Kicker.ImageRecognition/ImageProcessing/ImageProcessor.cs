using System.Drawing;
using Kicker.ImageRecognition.Masking;

namespace Kicker.ImageRecognition.ImageProcessing;

public class ImageProcessor : IDisposable
{
    private readonly Homography _homography;
    private readonly IMask[] _masks;

    public Bitmap? Bitmap { get; set; }
    public int Height => _homography.Height;
    public int Width => _homography.Width;

    public ImageProcessor(
        Homography homography,
        params IMask[] masks)
    {
        _homography = homography;
        _masks = masks;
    }

    public Color GetColor(
        int x,
        int y)
    {
        if (Bitmap is null)
            throw new InvalidOperationException(nameof(Bitmap) + "is null");

        var translatedPoint = _homography.Translate(x, y);
        if (IsMasked(translatedPoint.X, translatedPoint.Y))
            return Color.Black;

        return Bitmap.GetPixel(translatedPoint.X, translatedPoint.Y);
    }

    public byte GetGrayscale(
        int x,
        int y)
    {
        var color = GetColor(x, y);
        var grayscale = (byte) Math.Floor((0.3f * color.R) + (0.59f * color.G) + (0.11f * color.B));
        return grayscale;
    }

    public byte GetHighContrast(
        int x,
        int y,
        byte threshold)
    {
        var grayscale = GetGrayscale(x, y);
        return grayscale < threshold ? byte.MinValue : byte.MaxValue;
    }

    public void Dispose()
    {
        _homography?.Dispose();
    }

    private bool IsMasked(
        int x,
        int y)
    {
        foreach (var mask in _masks)
        {
            if (mask.Contains(x, y))
                return true;
        }

        return false;
    }
}