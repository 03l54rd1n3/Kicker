namespace Kicker.ImageRecognition.Imaging;

public static class ColorExtensions
{
    public static System.Drawing.Color ToSystemDrawing(
        this Color color)
        => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

    public static bool IsCloseTo(
        this Color color,
        Color other,
        byte threshold)
    {
        if (Math.Abs(color.R - other.R) > threshold)
            return false;

        if (Math.Abs(color.G - other.G) > threshold)
            return false;

        if (Math.Abs(color.B - other.B) > threshold)
            return false;

        return Math.Abs(color.A - other.A) <= threshold;
    }
}