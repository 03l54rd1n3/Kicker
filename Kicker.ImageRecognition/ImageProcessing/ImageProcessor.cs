using Kicker.ImageRecognition.Imaging;
using Kicker.ImageRecognition.Masking;

namespace Kicker.ImageRecognition.ImageProcessing;

public class ImageProcessor : IDisposable
{
    private const byte ColorFilterThreshold = 25;
    private readonly List<IMask> _masks = new();
    private readonly List<(Color Input, Color Output)> _colorFilters = new();
    private Homography? _homography;

    public IImage? Image { get; set; }
    public short Height => _homography!.Height;
    public short Width => _homography!.Width;

    public ImageProcessor SetHomography(
        Homography homography)
    {
        _homography = homography;
        return this;
    }

    public ImageProcessor AddMasks(
        params IMask[] masks)
    {
        _masks.AddRange(masks);
        return this;
    }

    public ImageProcessor AddColorFilters(
        params (Color Input, Color Output)[] colorFilters)
    {
        _colorFilters.AddRange(colorFilters);
        return this;
    }

    public Color GetColor(
        short x,
        short y)
    {
        if (Image is null)
            throw new InvalidOperationException(nameof(Image) + "is null");

        var translatedPoint = _homography.Translate(x, y);
        if (IsMasked(translatedPoint.X, translatedPoint.Y))
            return Color.Black;

        var color = Image.GetPixel(translatedPoint.X, translatedPoint.Y);
        return ApplyColorFilters(color, ColorFilterThreshold);
    }

    public byte GetGrayscale(
        short x,
        short y)
    {
        var color = GetColor(x, y);
        var grayscale = (byte) Math.Floor((0.3f * color.R) + (0.59f * color.G) + (0.11f * color.B));
        return grayscale;
    }

    public byte GetHighContrast(
        short x,
        short y,
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
        short x,
        short y)
    {
        foreach (var mask in _masks)
        {
            if (mask.Contains(x, y))
                return true;
        }

        return false;
    }

    private Color ApplyColorFilters(
        Color color,
        byte threshold)
    {
        foreach (var colorFilter in _colorFilters)
        {
            if (color.IsCloseTo(colorFilter.Input, threshold))
                return colorFilter.Output;
        }

        return color;
    }
}