using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Kicker.ImageRecognition.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class HomographyCalculationBenchmarks
{
    [Benchmark]
    public void FullCalculation()
    {
        var homography = new Homography()
            .SetOriginFrame(new(447, 185), new(1123, 70), new(1316, 930), new(603, 1060))
            .SetTargetFrame(700, 1000)
            .CalculateHomographyMatrix()!
            .CalculateTranslationMap()!;
        
        homography.Dispose();
    }
}