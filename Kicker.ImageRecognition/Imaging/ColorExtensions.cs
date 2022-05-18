namespace Kicker.ImageRecognition.Imaging;

public static class ColorExtensions
{
    public static System.Drawing.Color ToSystemDrawing(
        this Color color)
        => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
}