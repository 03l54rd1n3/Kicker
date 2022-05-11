using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

internal class HomographyTransformation : ImageTransformationBase
{
    private readonly IImageTransformation _source;
    private readonly Homography _homography;

    public override int Width => _homography.Width;
    public override int Height => _homography.Height;

    public HomographyTransformation(
        IImageTransformation source,
        Homography homography)
    {
        _source = source;
        _homography = homography;
    }

    public override Color At(
        int x,
        int y)
    {
        AssertBounds(x, y);
        
        var pointInOrigin = _homography.Translate(x, y);
        return _source.At(pointInOrigin.X, pointInOrigin.Y);
    }

    protected override void Dispose(
        bool disposing)
    {
        _homography.Dispose();
        _source.Dispose();
    }
}