using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

internal abstract class ImageTransformationBase : IImageTransformation
{
    public virtual int Width { get; }
    public virtual int Height { get; }

    protected virtual void AssertBounds(
        int x,
        int y)
    {
        if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
            throw new InvalidOperationException("Unsupported point");
    }

    public abstract Color At(
        int x,
        int y);

    public virtual Bitmap ToBitmap()
    {
        var bitmap = new Bitmap(Width, Height);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                bitmap.SetPixel(x, y, At(x, y));
            }
        }

        return bitmap;
    }
}