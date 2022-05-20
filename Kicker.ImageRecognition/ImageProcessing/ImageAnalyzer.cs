using System.Collections.Concurrent;
using Kicker.ImageRecognition.Imaging;
using Kicker.ImageRecognition.Masking;
using Kicker.Shared;

namespace Kicker.ImageRecognition.ImageProcessing;

public class ImageAnalyzer : IDisposable
{
    private readonly ConcurrentDictionary<Bar, float> _halfBarLengths = new();

    private ImageProcessor? _oneImageProcessor;
    private ImageProcessor? _twoImageProcessor;
    private ImageProcessor? _fiveImageProcessor;
    private ImageProcessor? _threeImageProcessor;
    private ImageProcessor? _fullImageProcessor;
    private IImage? _image;

    public IImage? Image
    {
        get => _image;
        set
        {
            _image = value;
            _oneImageProcessor.Image = value;
            _twoImageProcessor.Image = value;
            _fiveImageProcessor.Image = value;
            _threeImageProcessor.Image = value;
        }
    }

    public void InitializeBar(
        Bar bar,
        float length,
        Homography homography,
        params IMask[] masks)
    {
        _halfBarLengths[bar] = length / 2;

        var imageProcessor =
            new ImageProcessor()
                .SetHomography(homography)
                .AddMasks(masks);

        _ = bar switch
        {
            Bar.One => _oneImageProcessor = imageProcessor,
            Bar.Two => _twoImageProcessor = imageProcessor,
            Bar.Five => _fiveImageProcessor = imageProcessor,
            Bar.Three => _threeImageProcessor = imageProcessor,
            _ => throw new ArgumentOutOfRangeException(nameof(bar), bar, null)
        };
    }

    public void InitializeFullImageProcessing(
        Homography homography,
        short ballRadius,
        params IMask[] masks)
    {
        _fullImageProcessor =
            new ImageProcessor()
                .SetHomography(homography)
                .AddMasks(masks);

        var circleFinder = new HoughTransformCircleFinder(_fullImageProcessor, ballRadius);
    }

    public float FindBarPosition(
        Bar bar)
    {
        const short minimumPixelThreshold = 5;
        const short xOffset = 19;
        const byte colorThreshold = 100;
        var imageProcessor = GetBarImageProcessor(bar) ?? throw new InvalidOperationException("Bar not initialized");

        for (short y = 0; y < imageProcessor.Height / 2; y++)
        for (byte side = 0; side < 2; side++)
        {
            var currentY = unchecked((short) (side == 0 ? y : imageProcessor.Height - 1 - y));
            var x = xOffset;
            for (; x < xOffset + minimumPixelThreshold; x++)
            {
                var grayscale = imageProcessor.GetHighContrast(x, currentY, colorThreshold);
                if (grayscale != 0)
                    break;
            }

            if (x == xOffset + minimumPixelThreshold)
                return CalculateBarPosition(bar, imageProcessor.Height, currentY);
        }

        return float.NaN;
    }

    private ImageProcessor? GetBarImageProcessor(
        Bar bar)
        => bar switch
        {
            Bar.One => _oneImageProcessor,
            Bar.Two => _twoImageProcessor,
            Bar.Five => _fiveImageProcessor,
            Bar.Three => _threeImageProcessor,
            _ => throw new ArgumentOutOfRangeException(nameof(bar), bar, null),
        };

    private float CalculateBarPosition(
        Bar bar,
        short height,
        short y)
    {
        var offset = _halfBarLengths[bar];
        var position = (float) y / height + offset;

        if (y > height / 2)
            position = 1 - position;

        return position;
    }

    public void Dispose()
    {
        _oneImageProcessor?.Dispose();
        _twoImageProcessor?.Dispose();
        _fiveImageProcessor?.Dispose();
        _threeImageProcessor?.Dispose();
        _fullImageProcessor?.Dispose();
    }
}