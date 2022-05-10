using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

internal class MemorizedImageTransformation : ImageTransformationBase
{
    private readonly IImageTransformation _source;
    private readonly Bitmap _bitmap;
    private int _pixelsFilled;
    
    public override int Width => _source.Width;
    public override int Height => _source.Height;

    public MemorizedImageTransformation(
        IImageTransformation source)
    {
        _source = source;
        _bitmap = new Bitmap(Width, Height);
        var graphics = Graphics.FromImage(_bitmap);
        graphics.Clear(Color.Transparent);
    }

    public override Color At(
        int x,
        int y)
    {
        var color = _bitmap.GetPixel(x, y);
        if (color == Color.Transparent)
        {
            color = _source.At(x, y);
            _bitmap.SetPixel(x, y, color);
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
                    return _bitmap;

                At(x, y);
            }
        }

        return _bitmap;
    }
}