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
public class ImageAnalyzerFindBarPositionBenchmarks
{
    private IImage? _image;
    private ImageAnalyzer? _imageAnalyzer;

    [ParamsAllValues]
    public bool UseDirectBitmap { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var bitmap = (Bitmap) Bitmap.FromFile(@"C:\Users\mmertens\OneDrive - cleverbridge.com\Desktop\kicker (Small).jpeg");
        _image = new BitmapWrapper(bitmap);

        if (UseDirectBitmap)
        {
            var directBitmap = new DirectBitmap(_image.Width, _image.Height);
            for (short y = 0; y < _image.Height; y++)
            for (short x = 0; x < _image.Width; x++)
                directBitmap.SetPixel(x, y, _image.GetPixel(x, y));
            _image.Dispose();
            _image = directBitmap;
        }

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
        _imageAnalyzer.InitializeBar(Bar.One, (float) 210 / 457, maxDistanceHomography);
        _imageAnalyzer.InitializeBar(Bar.Two, (float) 210 / 457, shortDistanceHomography);
        _imageAnalyzer.InitializeBar(Bar.Five, (float) 210 / 457, maxDistanceHomography);
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
    public float[] Consecutive()
    {
        _imageAnalyzer.Image = _image;
        var floats = new float[4];

        floats[0] = _imageAnalyzer!.FindBarPosition(Bar.One);
        floats[1] = _imageAnalyzer!.FindBarPosition(Bar.Two);
        floats[2] = _imageAnalyzer!.FindBarPosition(Bar.Five);
        floats[3] = _imageAnalyzer!.FindBarPosition(Bar.Three);

        return floats;
    }

    [Benchmark]
    public async Task<float[]> WhenAll()
    {
        _imageAnalyzer.Image = _image;
        var float1Task = Task.Run(() => _imageAnalyzer!.FindBarPosition(Bar.One));
        var float2Task = Task.Run(() => _imageAnalyzer!.FindBarPosition(Bar.Two));
        var float3Task = Task.Run(() => _imageAnalyzer!.FindBarPosition(Bar.Five));
        var float4Task = Task.Run(() => _imageAnalyzer!.FindBarPosition(Bar.Three));

        return await Task.WhenAll(float1Task, float2Task, float3Task, float4Task);
    }

    [Benchmark]
    public async Task<float[]> ParallelInvoke()
    {
        _imageAnalyzer.Image = _image;
        var floats = new float[4];

        Parallel.Invoke(
            () => floats[0] = _imageAnalyzer!.FindBarPosition(Bar.One),
            () => floats[1] = _imageAnalyzer!.FindBarPosition(Bar.Two),
            () => floats[2] = _imageAnalyzer!.FindBarPosition(Bar.Five),
            () => floats[3] = _imageAnalyzer!.FindBarPosition(Bar.Three));

        return floats;
    }
}