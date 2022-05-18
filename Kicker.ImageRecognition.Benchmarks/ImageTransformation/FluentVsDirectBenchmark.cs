using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.ImageProcessing;
using Kicker.ImageRecognition.ImageTransformation;

namespace Kicker.ImageRecognition.Benchmarks.ImageTransformation;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FluentVsDirectBenchmark
{
    private Homography _homography;
    private Bitmap? _bitmap;
    private IImageTransformation _imageTransformation;
    private ImageProcessor _imageProcessor;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Pictures\Profilbild.jpg");
        _homography = GetHomography();
        _imageProcessor = new ImageProcessor(_homography);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _bitmap?.Dispose();
        _homography?.Dispose();
        _imageTransformation?.Dispose();
        _imageProcessor?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public Color Fluent()
    {
        _imageTransformation = _bitmap
            .AsImageTransformation()
            .AsHomographyTranslated(_homography)
            .AsGrayscale();

        var color = Color.White;

        for (var y = 0; y < _imageTransformation.Height; y++)
        for (var x = 0; x < _imageTransformation.Width; x++)
            if (_imageTransformation.At(x, y) == Color.Black)
                color = Color.Black;

        return color;
    }

    [Benchmark]
    public byte Direct()
    {
        _imageProcessor.Bitmap = _bitmap;
        byte color = 255;

        for (var y = 0; y < _imageProcessor.Height; y++)
        for (var x = 0; x < _imageProcessor.Width; x++)
            if (_imageProcessor.GetGrayscale(x, y) == 0)
                color = 0;

        return color;
    }

    private Homography GetHomography()
    {
        var topLeft = new Point(40, 170);
        var topRight = new Point(260, 50);
        var bottomRight = new Point(450, 310);
        var bottomLeft = new Point(230, 470);

        return new Homography()
            .SetOriginFrame(topLeft, topRight, bottomRight, bottomLeft)
            .SetTargetFrame(300, 380)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;
    }
}