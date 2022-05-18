namespace Kicker.ImageRecognition.Masking;

public interface IMask : IDisposable
{
    bool IsCalculated { get; }

    bool Contains(
        int x,
        int y);

    void Calculate(
        int width,
        int height);
}