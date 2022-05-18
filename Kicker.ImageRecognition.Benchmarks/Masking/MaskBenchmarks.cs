using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.Masking;

namespace Kicker.ImageRecognition.Benchmarks.Masking;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class MaskBenchmarks
{
    private const int ImageSize = 100;
    private IMask _circleMask;
    private IMask _ringMask;
    private IMask _polygonMask;

    [ParamsAllValues]
    public bool Calculate { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _circleMask = new CircleMask(5, 5, ImageSize / 10);
        _ringMask = new RingMask(ImageSize / 2, ImageSize / 2, 10, 15);
        _polygonMask = GetPolygonMask();

        if (Calculate)
        {
            _circleMask.Calculate(ImageSize, ImageSize);
            _ringMask.Calculate(ImageSize, ImageSize);
            _polygonMask.Calculate(ImageSize, ImageSize);
        }
    }

    [Benchmark]
    public int Circle()
        => Benchmark(_circleMask);

    [Benchmark]
    public int Ring()
        => Benchmark(_ringMask);

    [Benchmark]
    public int Polygon()
        => Benchmark(_polygonMask);

    private int Benchmark(
        IMask mask)
    {
        var count = 0;

        for (var y = 0; y < ImageSize; y++)
        for (var x = 0; x < ImageSize; x++)
            if (mask.Contains(x, y))
                count++;

        return count;
    }

    private static PolygonMask GetPolygonMask()
    {
        var points = new Point[]
        {
            new(80, 10),
            new(95, 20),
            new(80, 30),
            new(95, 40),
            new(80, 40),
            new(70, 0),
        };

        return new PolygonMask(ImageSize, points);
    }
}