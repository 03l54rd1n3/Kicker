using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Kicker.ImageRecognition;
using Kicker.ImageRecognition.ImageTransformation;

var stopWatch = new Stopwatch();
stopWatch.Start();
using var homography = new Homography()
    .SetOriginFrame(new(447, 185), new(1123, 70), new(1316, 930), new(603, 1060))
    .SetTargetFrame(700, 1000)
    .CalculateHomographyMatrix()!
    .CalculateTranslationMap()!;
stopWatch.Stop();
Console.WriteLine(stopWatch.ElapsedMilliseconds);

var sourceImagePath = @"C:\Users\login\OneDrive\Desktop\cards.jfif";
if (!File.Exists(sourceImagePath))
    throw new Exception("wtf");

var originalBitmap = new Bitmap(sourceImagePath);

var translatedBitmap = originalBitmap.AsImageTransformation()
                                     .AsHomographyTranslated(homography)
                                     .ToBitmap();

translatedBitmap.Save(@"C:\Users\login\OneDrive\Desktop\cards2.jpeg", ImageFormat.Jpeg);