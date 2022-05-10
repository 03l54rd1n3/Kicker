using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Kicker.ImageRecognition.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.SlowestToFastest)]
[RankColumn]
public class HomographyBenchmarks
{
    [Benchmark]
    public void FullCalculation()
    {
        new Homography()
            .SetOriginFrame(new(447, 185), new(1123, 70), new(1316, 930), new(603, 1060))
            .SetTargetFrame(700, 1000)
            .CalculateHomographyMatrix()
            .CalculateTranslationMap();
    }
}