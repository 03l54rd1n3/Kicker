using System.Drawing;
using System.Drawing.Imaging;
using Kicker.ImageRecognition;
using Kicker.ImageRecognition.ImageProcessing;
using Kicker.ImageRecognition.Imaging;
using Kicker.Shared;
using Color = System.Drawing.Color;

const int size = 100;

var homography = new Homography()
    .SetOriginFrame(
        new PointS(147, 18),
        new PointS(191, 17),
        new PointS(200, 473),
        new PointS(155, 475))
    .SetTargetFrame(45, 457)
    .CalculateHomographyMatrix()!
    .CalculateTranslationMap()!;

using var imageProcessor = new ImageProcessor(homography);
var bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\kicker 2 (Small).jpeg");
imageProcessor.Image = new BitmapWrapper(bitmap);
bitmap = new Bitmap(45, 457);

for (short y = 0; y < imageProcessor.Height; y++)
for (short x = 0; x < imageProcessor.Width; x++)
{
    var grayscale = imageProcessor.GetHighContrast(x, y, 100);
    var color = grayscale == 0 ? Color.Black : Color.White;
    bitmap.SetPixel(x, y, color);
}

bitmap.Save(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\kicker 2 (Small).png", ImageFormat.Png);

var imageAnalyzer = new ImageAnalyzer();
imageAnalyzer.InitializeBar(Bar.Two, (float) 210 / 457, homography);
imageAnalyzer.Image = imageProcessor.Image;

var position = imageAnalyzer.FindBarPosition(Bar.Two);
Console.WriteLine(position);