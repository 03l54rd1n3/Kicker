using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.ImageProcessing;
using Kicker.ImageRecognition.Imaging;
using Kicker.Shared;

namespace Kicker.ImageRecognition.Benchmarks.ImageProcessing;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class HoughTransformCircleFinderBenchmarks
{
    private IImage? _image;
    private ImageProcessor? _imageProcessor;
    private HoughTransformCircleFinder? _sut;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\edge_detection_test.png");
        using var wrappedBitmap = new BitmapWrapper(bitmap);
        _image = new DirectBitmap(wrappedBitmap.Width, wrappedBitmap.Height);
        CopyTo(wrappedBitmap, _image);

        var homography = GetHomography(_image.Width, _image.Height);
        _imageProcessor = new ImageProcessor().SetHomography(homography);
        _imageProcessor.Image = _image;

        _sut = new HoughTransformCircleFinder(_imageProcessor, 15);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _image?.Dispose();
        _imageProcessor?.Dispose();
    }

    [Benchmark]
    public PointS[] EnumerateEdges()
    {
        var points = _sut!.EnumerateEdges(0, 0, _imageProcessor!.Width, _imageProcessor.Height, 200);
        return points.ToArray();
    }

    [Benchmark]
    public PointS FindCircle()
    {
        var point = _sut!.FindCircle(0, 0, _imageProcessor!.Width, _imageProcessor.Height, 200);
        return point;
    }

    private Homography GetHomography(
        short width,
        short height)
        => new Homography()
            .SetOriginFrame(
                new PointS(0, 0),
                new PointS(unchecked((short)(width - 1)), 0),
                new PointS(unchecked((short)(width - 1)), unchecked((short)(height - 1))),
                new PointS(0, unchecked((short)(height -1))))
            .SetTargetFrame(150, height)
            .CalculateHomographyMatrix()
            .CalculateTranslationMap();

    private void CopyTo(
        IImage src,
        IImage dest)
    {
        for (short y = 0; y < src.Height; y++)
        for (short x = 0; x < src.Width; x++)
            dest.SetPixel(x, y, src.GetPixel(x, y));
    }
}