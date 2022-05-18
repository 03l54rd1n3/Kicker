using System.Drawing;
using System.Drawing.Imaging;
using Kicker.ImageRecognition;
using Kicker.ImageRecognition.ImageProcessing;
using Kicker.ImageRecognition.Masking;
using Kicker.Shared;

const int size = 100;

var homography = new Homography()
    .SetOriginFrame(
        new Point(147, 18),
        new Point(191, 17),
        new Point(200, 473),
        new Point(155, 475))
    .SetTargetFrame(45, 457)
    .CalculateHomographyMatrix()!
    .CalculateTranslationMap()!;

using var imageProcessor = new ImageProcessor(homography);
imageProcessor.Bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\kicker 2 (Small).jpeg");

var bitmap = new Bitmap(45, 457);

for (var y = 0; y < imageProcessor.Height; y++)
for (var x = 0; x < imageProcessor.Width; x++)
{
    var grayscale = imageProcessor.GetHighContrast(x, y, 100);
    var color = grayscale == 0 ? Color.Black : Color.White;
    bitmap.SetPixel(x, y, color);
}

bitmap.Save(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\kicker 2 (Small).png", ImageFormat.Png);

var imageAnalyzer = new ImageAnalyzer();
imageAnalyzer.InitializeBar(Bar.Two, (float) 210 / 457, homography);
imageAnalyzer.Bitmap = imageProcessor.Bitmap;

var position = imageAnalyzer.FindBarPosition(Bar.Two);
Console.WriteLine(position);