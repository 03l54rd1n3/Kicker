using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

internal class BitmapTransformation : ImageTransformationBase
{
    private readonly Bitmap _bitmap;

    public override int Width => _bitmap.Width;
    public override int Height => _bitmap.Height;
    
    public BitmapTransformation(
        Bitmap bitmap)
    {
        _bitmap = bitmap;
    }

    public override Color At(
        int x,
        int y)
    {
        AssertBounds(x, y);
        return _bitmap.GetPixel(x, y);
    }

    public override Bitmap ToBitmap()
        => _bitmap;
}