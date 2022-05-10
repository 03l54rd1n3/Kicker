using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Kicker.ImageRecognition;
using Kicker.ImageRecognition.ImageTransformation;

var stopWatch = new Stopwatch();
stopWatch.Start();
var homography = new Homography()
    .SetOriginFrame(new(450, 180), new(1120, 70), new(1310, 930), new(600, 1060))
    .SetTargetFrame(700, 1000)
    .CalculateHomographyMatrix()
    .CalculateTranslationMap();
stopWatch.Stop();
Console.WriteLine(stopWatch.ElapsedMilliseconds);

var originalBitmap = new Bitmap("~/Desktop/cards.jpeg");

var translatedBitmap = originalBitmap.AsImageTransformation()
                                     .AsHomographyTranslated(homography)
                                     .ToBitmap();

translatedBitmap.Save("~/Desktop/cards2.jpeg", ImageFormat.Jpeg);