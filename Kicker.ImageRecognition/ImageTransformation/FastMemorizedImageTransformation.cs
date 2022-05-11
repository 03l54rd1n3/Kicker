using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

internal class FastMemorizedImageTransformation : ImageTransformationBase
{
    private readonly IImageTransformation _source;
    private readonly DirectBitmap _directBitmap;
    private int _pixelsFilled;
    
    public override int Width => _source.Width;
    public override int Height => _source.Height;

    public FastMemorizedImageTransformation(
        IImageTransformation source)
    {
        _source = source;
        _directBitmap = new DirectBitmap(Width, Height);
        using var graphics = Graphics.FromImage(_directBitmap.Bitmap);
        graphics.Clear(Color.Transparent);
    }

    public override Color At(
        int x,
        int y)
    {
        var color = _directBitmap.GetPixel(x, y);
        if (color.A == 0)
        {
            color = _source.At(x, y);
            _directBitmap.SetPixel(x, y, color);
            _pixelsFilled++;
        }

        return color;
    }

    public override Bitmap ToBitmap()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (_pixelsFilled == Width * Height)
                    return _directBitmap.Bitmap;

                At(x, y);
            }
        }

        return _directBitmap.Bitmap;
    }

    protected override void Dispose(
        bool disposing)
    {
        _directBitmap.Dispose();
        _source.Dispose();
    }
}