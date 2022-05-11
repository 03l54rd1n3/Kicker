using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.ImageTransformation;

namespace Kicker.ImageRecognition.Benchmarks.ImageTransformation;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class MemorizationBenchmarks
{
    private const int FixedSize = 700;
    private IImageTransformation _imageTransformation;
    
    [Params(1, 2, 3, 4, 5, 10)]
    public int Iterations { get; set; }
    
    [Params(100, 500, 700, 1000, 5000)]
    public int ImageSize { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var bitmap = new Bitmap(FixedSize, FixedSize);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Blue);

        var homography = GetHomography();
        _imageTransformation = bitmap
            .AsImageTransformation()
            .AsHomographyTranslated(homography);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _imageTransformation.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void Raw()
    {
        Benchmark(_imageTransformation);
    }
    
    [Benchmark]
    public void Memorized()
    {
        var imageTransformation = _imageTransformation.AsMemorized();
        Benchmark(imageTransformation);
    }
    
    [Benchmark]
    public void FastMemorized()
    {
        var imageTransformation = _imageTransformation.AsFastMemorized();
        Benchmark(imageTransformation);
    }

    private void Benchmark(
        IImageTransformation imageTransformation)
    {
        for (var i = 0; i < Iterations; i++)
        for (var y = 0; y < ImageSize; y++)
        for (var x = 0; x < ImageSize; x++)
        {
            var color = imageTransformation.At(x, y);
            if (color.B != Color.Blue.B)
                throw new Exception();
        }
    }

    private Homography GetHomography()
    {
        var topLeft = new Point(0, 0);
        var topRight = new Point(FixedSize -1, 0);
        var bottomRight = new Point(FixedSize - 1, FixedSize - 1);
        var bottomLeft = new Point(0, FixedSize -1);
        
        return new Homography()
            .SetOriginFrame(topLeft, topRight, bottomRight, bottomLeft)
            .SetTargetFrame(ImageSize, ImageSize)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;
    }
}