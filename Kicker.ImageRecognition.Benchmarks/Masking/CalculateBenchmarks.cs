using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.Masking;

namespace Kicker.ImageRecognition.Benchmarks.Masking;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class CalculateBenchmarks
{
    private const int ImageSize = 100;

    [Benchmark]
    public IMask Circle()
        => Benchmark(new CircleMask(5, 5, ImageSize / 10));

    [Benchmark]
    public IMask Ring()
        => Benchmark(new RingMask(ImageSize / 2, ImageSize / 2, 10, 15));

    [Benchmark]
    public IMask Polygon()
        => Benchmark(GetPolygonMask());

    private IMask Benchmark(
        IMask mask)
    {
        mask.Calculate(ImageSize, ImageSize);
        return mask;
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