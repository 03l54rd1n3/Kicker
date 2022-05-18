using System.Drawing;
using Kicker.ImageRecognition.Imaging;

namespace Kicker.ImageRecognition.ImageTransformation;

internal class DirectBitmapTransformation : ImageTransformationBase
{
    private readonly DirectBitmap _directBitmap;

    public override int Width => _directBitmap.Width;
    public override int Height => _directBitmap.Height;
    
    public DirectBitmapTransformation(
        DirectBitmap directBitmap)
    {
        _directBitmap = directBitmap;
    }

    public override Color At(
        int x,
        int y)
    {
        AssertBounds(x, y);
        return _directBitmap.GetPixel(x, y);
    }

    public override Bitmap? ToBitmap()
        => _directBitmap.Bitmap;

    protected override void Dispose(
        bool disposing)
    {
        _directBitmap?.Dispose();
    }
}