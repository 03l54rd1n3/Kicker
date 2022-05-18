using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.Shared;

namespace Kicker.ImageRecognition.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class HomographyTransformBenchmarks
{
    private Homography _homography;

    [Params(50, 100, 500, 700, 1000, 10000)]
    public short Size { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var topLeft = new PointS();
        unchecked
        {
            var topRight = new PointS((short) (Size - 1), (short) (Size / 5 - 1));
            var bottomRight = new PointS((short) (Size - 1), (short) (Size + Size / 5 - 1));
            var bottomLeft = new PointS(0, (short) (Size - 1));

            _homography = new Homography()
                .SetOriginFrame(topLeft, topRight, bottomRight, bottomLeft)
                .SetTargetFrame(Size, Size)
                .CalculateHomographyMatrix()!
                .CalculateTranslationMap()!;
        }
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _homography.Dispose();
    }

    [Benchmark]
    public void Transform()
    {
        for (short y = 0; y < Size; y++)
        for (short x = 0; x < Size; x++)
            _homography.Translate(x, y);
    }
}