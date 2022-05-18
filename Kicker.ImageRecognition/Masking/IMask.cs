namespace Kicker.ImageRecognition.Masking;

public interface IMask : IDisposable
{
    bool IsCalculated { get; }

    bool Contains(
        short x,
        short y);

    void Calculate(
        short width,
        short height);
}