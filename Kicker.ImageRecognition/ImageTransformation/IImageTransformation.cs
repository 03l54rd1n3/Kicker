using System.Drawing;

namespace Kicker.ImageRecognition.ImageTransformation;

public interface IImageTransformation
{
    int Width { get; }
    int Height { get; }
    
    Color At(
        int x,
        int y);

    Bitmap ToBitmap();
}