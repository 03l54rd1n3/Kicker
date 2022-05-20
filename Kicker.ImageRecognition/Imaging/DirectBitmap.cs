using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Kicker.ImageRecognition.Imaging;

public class DirectBitmap : IImage
{
    public Bitmap Bitmap { get; private set; }
    public int[] Bits { get; private set; }
    public bool Disposed { get; private set; }
    public short Height { get; }
    public short Width { get; }

    protected GCHandle BitsHandle { get; private set; }

    public DirectBitmap(
        short width,
        short height)
    {
        Width = width;
        Height = height;
        Bits = new int[width * height];
        BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
        Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
    }

    public Color GetPixel(
        short x,
        short y)
    {
        var index = x + y * Width;
        var col = Bits[index];
        return new Color(col);
    }

    public void SetPixel(
        short x,
        short y,
        Color color)
    {
        var index = x + y * Width;
        var col = color.Value;

        Bits[index] = col;
    }

    public void Dispose()
    {
        if (Disposed)
            return;
        Disposed = true;
        Bitmap.Dispose();
        BitsHandle.Free();
    }
}