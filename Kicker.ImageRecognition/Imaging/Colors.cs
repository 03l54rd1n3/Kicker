namespace Kicker.ImageRecognition.Imaging;

public readonly partial struct Color
{
    public static readonly Color Black = new(byte.MinValue, byte.MinValue, byte.MinValue);

    public static readonly Color White = new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
}