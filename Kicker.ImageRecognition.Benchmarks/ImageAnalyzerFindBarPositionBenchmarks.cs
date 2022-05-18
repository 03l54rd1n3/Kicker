using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.ImageProcessing;
using Kicker.ImageRecognition.Imaging;
using Kicker.Shared;

namespace Kicker.ImageRecognition.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ImageAnalyzerFindBarPositionBenchmarks
{
    private IImage? _image;
    private ImageAnalyzer? _imageAnalyzer;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\kicker (Small).jpeg");
        _image = new BitmapWrapper(bitmap);

        var shortDistanceHomography = new Homography()
            .SetOriginFrame(
                new PointS(666, 9),
                new PointS(708, 9),
                new PointS(731, 466),
                new PointS(687, 467))
            .SetTargetFrame(45, 457)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;

        var maxDistanceHomography = new Homography()
            .SetOriginFrame(
                new PointS(147, 18),
                new PointS(191, 17),
                new PointS(200, 473),
                new PointS(155, 475))
            .SetTargetFrame(45, 457)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;

        _imageAnalyzer = new ImageAnalyzer();
        _imageAnalyzer.InitializeBar(Bar.One, (float) 210 / 457, shortDistanceHomography);
        _imageAnalyzer.InitializeBar(Bar.Two, (float) 210 / 457, shortDistanceHomography);
        _imageAnalyzer.InitializeBar(Bar.Five, (float) 210 / 457, shortDistanceHomography);
        _imageAnalyzer.InitializeBar(Bar.Three, (float) 210 / 457, maxDistanceHomography);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _image?.Dispose();
        _imageAnalyzer?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public float ShortDistance()
    {
        _imageAnalyzer.Image = _image;
        return _imageAnalyzer!.FindBarPosition(Bar.Two);
    }

    [Benchmark]
    public float MaxDistance()
    {
        _imageAnalyzer.Image = _image;
        return _imageAnalyzer!.FindBarPosition(Bar.Three);
    }

    [Benchmark]
    public (float, float) Consecutive()
    {
        _imageAnalyzer.Image = _image;
        var float1 = _imageAnalyzer!.FindBarPosition(Bar.Two);
        var float2 = _imageAnalyzer!.FindBarPosition(Bar.Three);
        return (float1, float2);
    }

    [Benchmark]
    public async Task<(float, float)> WhenAll()
    {
        _imageAnalyzer.Image = _image;
        var float1Task = Task.Run(() => _imageAnalyzer!.FindBarPosition(Bar.Two));
        var float2Task = Task.Run(() => _imageAnalyzer!.FindBarPosition(Bar.Three));

        await Task.WhenAll(float1Task, float2Task);
        return (float1Task.Result, float2Task.Result);
    }
}