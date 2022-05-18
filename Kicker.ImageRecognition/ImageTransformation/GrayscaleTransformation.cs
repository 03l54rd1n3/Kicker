using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

internal class GrayscaleTransformation : ImageTransformationBase
{
    private IImageTransformation _source;

    public override int Width => _source.Width;
    public override int Height => _source.Height;

    public GrayscaleTransformation(
        IImageTransformation source)
    {
        _source = source;
    }

    public override Color At(
        int x,
        int y)
    {
        var color = _source.At(x, y);
        var grayscale = (byte)Math.Floor((0.3f * color.R) + (0.59f * color.G) + (0.11f * color.B));
        return Color.FromArgb(grayscale, grayscale, grayscale);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected override void Dispose(
        bool disposing)
    {
        if (disposing)
            _source.Dispose();
    }
}