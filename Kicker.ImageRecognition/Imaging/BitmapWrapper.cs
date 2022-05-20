using System.Drawing;

namespace Kicker.ImageRecognition.Imaging;

public class BitmapWrapper : IImage
{
    private object _lock = new();
    private readonly Bitmap _bitmap;

    public short Width { get; }
    public short Height { get; }

    public BitmapWrapper(
        Bitmap bitmap)
    {
        _bitmap = bitmap;
        Width = unchecked((short) bitmap.Width);
        Height = unchecked((short) bitmap.Height);
    }

    public Color GetPixel(
        short x,
        short y)
    {
        System.Drawing.Color color;
        lock (_lock)
        {
            color = _bitmap.GetPixel(x, y);
        }

        return Color.FromSystemDrawing(color);
    }

    public void SetPixel(
        short x,
        short y,
        Color color)
    {
        var systemDrawingColor = color.ToSystemDrawing();
        lock (_lock)
        {
            _bitmap.SetPixel(x, y, systemDrawingColor);
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _bitmap?.Dispose();
        }
    }
}