using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.Imaging;

namespace Kicker.ImageRecognition.Benchmarks.Imaging;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ColorFromArgbBenchmarks
{
    private Random _random;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _random = new Random();
    }

    [Benchmark(Baseline = true)]
    public (byte, byte, byte, byte) SystemDrawing()
    {
        var argb = _random.Next();
        var color = System.Drawing.Color.FromArgb(argb);
        return (color.A, color.R, color.G, color.B);
    }

    [Benchmark]
    public (byte, byte, byte, byte) Bitwise()
    {
        var argb = _random.Next();
        var color = new Color(argb);
        return (color.A, color.R, color.G, color.B);
    }
}