namespace Kicker.ImageRecognition.Imaging;

public readonly partial struct Color : IEquatable<Color>, IEquatable<System.Drawing.Color>
{
    public byte A { get; }

    public byte R { get; }

    public byte G { get; }

    public byte B { get; }

    public bool IsTransparent => A == 0;

    public int Value
    {
        get
        {
            ReadOnlySpan<byte> span = stackalloc byte[4] { B, G, R, A };
            return BitConverter.ToInt32(span);
        }
    }

    public Color(
        byte r,
        byte g,
        byte b,
        byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(
        int argb)
    {
        const byte rShift = 16;
        const byte gShift = 8;
        const byte aShift = 24;

        R = unchecked((byte)(argb >> rShift));
        G = unchecked((byte)(argb >> gShift));
        B = unchecked((byte)argb);
        A = unchecked((byte)(argb >> aShift));
    }

    public bool Equals(
        Color other)
        => R == other.R &&
           G == other.G &&
           B == other.B &&
           A == other.A;

    public bool Equals(
        System.Drawing.Color other)
        => A == other.A &&
           R == other.R &&
           G == other.G &&
           B == other.B;

    public override bool Equals(
        object? obj)
    {
        if (obj is Color color)
            return Equals(color);

        return obj is System.Drawing.Color systemDrawing && Equals(systemDrawing);
    }

    public override int GetHashCode()
        => Value;

    public static bool operator ==(
        Color left,
        Color right)
        => left.Equals(right);

    public static bool operator !=(
        Color left,
        Color right)
        => !left.Equals(right);

    public static bool operator ==(
        Color left,
        System.Drawing.Color right)
        => left.Equals(right);

    public static bool operator !=(
        Color left,
        System.Drawing.Color right)
        => !left.Equals(right);

    public override string ToString()
        => $"ARGB=( {A}, {R}, {G}, {B} )";

    public static Color FromSystemDrawing(
        System.Drawing.Color color)
        => new(color.R, color.G, color.B, color.A);
}