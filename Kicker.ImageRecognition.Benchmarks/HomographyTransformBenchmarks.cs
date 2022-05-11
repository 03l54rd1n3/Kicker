using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Kicker.ImageRecognition.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class HomographyTransformBenchmarks
{
    private Homography _homography;
    
    [Params(50, 100, 500, 700, 1000, 10000)]
    public int Size { get; set; }
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var topLeft = new Point(0, 0);
        var topRight = new Point(Size -1, Size / 5 - 1);
        var bottomRight = new Point(Size - 1, Size + Size / 5 - 1);
        var bottomLeft = new Point(0, Size -1);
        
        _homography = new Homography()
            .SetOriginFrame(topLeft, topRight, bottomRight, bottomLeft)
            .SetTargetFrame(Size, Size)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _homography.Dispose();
    }
    
    [Benchmark]
    public void Transform()
    {
        for (var y = 0; y < Size; y++)
        for (var x = 0; x < Size; x++)
            _homography.Translate(x, y);
    }
}