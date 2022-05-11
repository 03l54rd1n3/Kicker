using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.ImageTransformation;

namespace Kicker.ImageRecognition.Benchmarks.ImageTransformation;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class BitmapBenchmarks
{
    private static Random _random = new();
    private static byte[] _colorBuffer = new byte[3];
    
    [ParamsSource(nameof(GetImageTransformationParams))]
    public object RootImageTransformationObject { get; set; }

    private IImageTransformation _rootImageTransformation;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _rootImageTransformation = RootImageTransformationObject as IImageTransformation ?? throw new InvalidOperationException();
    }
    
    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _rootImageTransformation.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void ClassicMemorized()
    {
        var bitmap = _rootImageTransformation.AsMemorized().ToBitmap();
        bitmap.Dispose();
    }
    
    [Benchmark]
    public void FastMemorized()
    {
        var bitmap = _rootImageTransformation.AsFastMemorized().ToBitmap();
        bitmap.Dispose();
    }

    public static IEnumerable<object> GetImageTransformationParams()
    {
        var sizes = new []{ 100, 700, 5000 };

        foreach (var size in sizes)
        {
            var bitmap = new Bitmap(size, size);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                bitmap.SetPixel(x, y, GetRandomColor());
            yield return bitmap.AsImageTransformation();

            var directBitmap = new DirectBitmap(size, size);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                directBitmap.SetPixel(x, y, GetRandomColor());
            yield return directBitmap.AsImageTransformation();
        }
    }

    private static Color GetRandomColor()
    {
        _random.NextBytes(_colorBuffer);
        var red = byte.Min(_colorBuffer[0], 254);
        var green = byte.Min(_colorBuffer[1], 254);
        var blue = byte.Min(_colorBuffer[2], 254);
        return Color.FromArgb(red, green, blue);
    }
}