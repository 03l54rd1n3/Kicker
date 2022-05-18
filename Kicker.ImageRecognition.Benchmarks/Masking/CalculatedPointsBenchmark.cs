using System.Collections;
using System.Drawing;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Kicker.ImageRecognition.Masking;

namespace Kicker.ImageRecognition.Benchmarks.Masking;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class CalculatedPointsBenchmark
{
    private const int ImageSize = 100;
    private HashSet<Point> _pointHashSet;
    private HashSet<(int, int)> _tupleHashSet;
    private List<Point> _pointList;
    private List<(int, int)> _tupleList;
    private Point[] _pointArray;
    private (int, int)[] _tupleArray;
    private HashSet<int> _intHashSet;
    private List<int> _intList;
    private int[] _intArray;

    [ParamsAllValues]
    public bool UseLinq { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _pointHashSet = new HashSet<Point>();
        _tupleHashSet = new HashSet<(int, int)>();
        _pointList = new List<Point>();
        _tupleList = new List<(int, int)>();
        _intHashSet = new HashSet<int>();
        _intList = new List<int>();

        var polygonMask = GetPolygonMask();

        for (var y = 0; y < ImageSize; y++)
        for (var x = 0; x < ImageSize; x++)
        {
            if (polygonMask.Contains(x, y))
            {
                var point = new Point(x, y);
                var tuple = (x, y);
                var @int = x * 10000 + y;

                _pointHashSet.Add(point);
                _pointList.Add(point);
                _tupleHashSet.Add(tuple);
                _tupleList.Add(tuple);
                _intHashSet.Add(@int);
                _intList.Add(@int);
            }
        }

        _pointArray = _pointList.ToArray();
        _tupleArray = _tupleList.ToArray();
        _intArray = _intList.ToArray();
    }

    [Benchmark(Baseline = true)]
    public int TupleHashSet()
    {
        return Benchmark(_tupleHashSet);
    }

    [Benchmark]
    public int TupleList()
    {
        return Benchmark(_tupleList);
    }

    [Benchmark]
    public int TupleArray()
    {
        return Benchmark(_tupleArray);
    }

    [Benchmark]
    public int PointHashSet()
    {
        return Benchmark(_pointHashSet);
    }

    [Benchmark]
    public int PointList()
    {
        return Benchmark(_pointList);
    }

    [Benchmark]
    public int PointArray()
    {
        return Benchmark(_pointArray);
    }

    [Benchmark]
    public int IntHashSet()
    {
        return Benchmark(_intHashSet);
    }

    [Benchmark]
    public int IntList()
    {
        return Benchmark(_intList);
    }

    [Benchmark]
    public int IntArray()
    {
        return Benchmark(_intArray);
    }

    private int Benchmark(
        ICollection<Point> collection)
    {
        var count = 0;

        for (var y = 0; y < ImageSize; y++)
        for (var x = 0; x < ImageSize; x++)
        {
            var point = new Point(x, y);
            if (UseLinq)
            {
                if (collection.Contains(point))
                    count++;
            }
            else
            {
                foreach (var _ in collection)
                    if (_ == point)
                    {
                        count++;
                        break;
                    }
            }
        }

        return count;
    }

    private int Benchmark(
        ICollection<(int, int)> collection)
    {
        var count = 0;

        for (var y = 0; y < ImageSize; y++)
        for (var x = 0; x < ImageSize; x++)
        {
            var tuple = (x, y);
            if (UseLinq)
            {
                if (collection.Contains(tuple))
                    count++;
            }
            else
            {
                foreach (var _ in collection)
                    if (_ == tuple)
                    {
                        count++;
                        break;
                    }
            }
        }

        return count;
    }

    private int Benchmark(
        ICollection<int> collection)
    {
        var count = 0;

        for (var y = 0; y < ImageSize; y++)
        for (var x = 0; x < ImageSize; x++)
        {
            var @int = x * 10000 + y;
            if (UseLinq)
            {
                if (collection.Contains(@int))
                    count++;
            }
            else
            {
                foreach (var _ in collection)
                    if (_ == @int)
                    {
                        count++;
                        break;
                    }
            }
        }

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