using System.Drawing;
using Kicker.ImageRecognition.ImageProcessing;
using Kicker.ImageRecognition.Imaging;
using Kicker.Shared;
using Snapshooter.Xunit;
using Color = Kicker.ImageRecognition.Imaging.Color;

namespace Kicker.ImageRecognition.Tests.ImageProcessing;

public class HoughTransformCircleFinderTests
{
    [Fact]
    public void EnumerateEdges_Snapshot_ReturnsCorrectEdges()
    {
        // Arrage
        var bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\edge_detection_test.png");
        using var wrappedBitmap = new BitmapWrapper(bitmap);
        using var directBitmap = new DirectBitmap((short) bitmap.Width, (short) bitmap.Height);
        CopyTo(wrappedBitmap, directBitmap);

        var homography = GetHomography(directBitmap.Width, directBitmap.Height);
        using var imageProcessor = new ImageProcessor().SetHomography(homography);
        imageProcessor.Image = directBitmap;

        var sut = new HoughTransformCircleFinder(imageProcessor, 20);

        // Act
        var returnedPoints = sut.EnumerateEdges(
            0,
            0,
            directBitmap.Width,
            directBitmap.Height,
            200).ToArray();

        // Assert
        Assert.All(returnedPoints, _ => Assert.Equal(Color.White, directBitmap.GetPixel(_.X, _.Y)));
        Snapshot.Match(returnedPoints);

        /*
        var edgeBitmap = new DirectBitmap(wrappedBitmap.Width, wrappedBitmap.Height);
        for (short y = 0; y < edgeBitmap.Height; y++)
        for (short x = 0; x < edgeBitmap.Width; x++)
        {
            var color = returnedPoints.Contains(new PointS(x, y)) ? Color.White : Color.Black;
            edgeBitmap.SetPixel(x, y, color);
        }
        edgeBitmap.Bitmap.Save(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\edge_detection_test_result.png", ImageFormat.Png);
        */
    }

    [Fact]
    public void FindCircle_Snapshot_ReturnsCorrectEdges()
    {
        // Arrage
        var bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\edge_detection_test.png");
        using var wrappedBitmap = new BitmapWrapper(bitmap);
        using var directBitmap = new DirectBitmap((short) bitmap.Width, (short) bitmap.Height);
        CopyTo(wrappedBitmap, directBitmap);

        var homography = GetHomography(directBitmap.Width, directBitmap.Height);
        using var imageProcessor = new ImageProcessor().SetHomography(homography);
        imageProcessor.Image = directBitmap;

        var sut = new HoughTransformCircleFinder(imageProcessor, 15);

        // Act
        var circleCenter = sut.FindCircle(
            0,
            0,
            directBitmap.Width,
            directBitmap.Height,
            200);

        // Assert
        Snapshot.Match(circleCenter);

        directBitmap.SetPixel(circleCenter.X, circleCenter.Y, Color.Green);
        directBitmap.Bitmap.Save(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\edge_detection_test_center.png", System.Drawing.Imaging.ImageFormat.Png);
    }

    private void CopyTo(
        IImage src,
        IImage dest)
    {
        for (short y = 0; y < src.Height; y++)
        for (short x = 0; x < src.Width; x++)
            dest.SetPixel(x, y, src.GetPixel(x, y));
    }

    private Homography GetHomography(
        short width,
        short height)
        => new Homography()
            .SetOriginFrame(
                new PointS(0, 0),
                new PointS(unchecked((short) (width - 1)), 0),
                new PointS(unchecked((short) (width - 1)), unchecked((short) (height - 1))),
                new PointS(0, unchecked((short) (height - 1))))
            .SetTargetFrame(150, height)
            .CalculateHomographyMatrix()
            .CalculateTranslationMap();
}