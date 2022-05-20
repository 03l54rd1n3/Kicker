namespace Kicker.ImageRecognition.Imaging;

public interface IImage : IDisposable
{
    short Width { get; }
    short Height { get; }

    Color GetPixel(
        short x,
        short y);

    void SetPixel(
        short x,
        short y,
        Color color);
}