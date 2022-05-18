namespace Kicker.ImageRecognition.Imaging;

public interface IImage : IDisposable
{
    int Width { get; }
    int Height { get; }

    Color GetPixel(
        short x,
        short y);

    void SetPixel(
        short x,
        short y,
        Color color);
}