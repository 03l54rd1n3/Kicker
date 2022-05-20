using System.Buffers;
using System.Collections.Concurrent;
using Kicker.Shared;

namespace Kicker.ImageRecognition.ImageProcessing;

public class HoughTransformCircleFinder
{
    private readonly ImageProcessor _imageProcessor;
    private readonly short _r;
    private readonly PointS[] _circlePoints;

    public HoughTransformCircleFinder(
        ImageProcessor imageProcessor,
        short r)
    {
        _imageProcessor = imageProcessor;
        _r = r;
        _circlePoints = CalculateCirclePoints(r);
    }

    public PointS FindCircle(
        short x,
        short y,
        short width,
        short height,
        byte threshold)
    {
        width = Math.Min(unchecked((short) (_imageProcessor.Width - x)), width);
        height = Math.Min(unchecked((short) (_imageProcessor.Height - x)), height);

        var accumulatorWidth = width + 2 * _r;
        var accumulatorHeight = height + 2 * _r;
        var accumulatorLength = accumulatorWidth * accumulatorHeight;
        var accumulator = ArrayPool<int>.Shared.Rent(accumulatorLength);

        var best = 0;
        var bestX = accumulatorWidth / 2;
        var bestY = accumulatorHeight / 2;

        foreach (var edgePoint in EnumerateEdges(
                     x,
                     y,
                     width,
                     height,
                     threshold))
        {
            foreach (var circlePoint in _circlePoints)
            {
                var accumulatorX = edgePoint.X + circlePoint.X + _r;
                var accumulatorY = edgePoint.Y + circlePoint.Y + _r;
                var accumulatorIndex = accumulatorX + accumulatorY * accumulatorWidth;
                Interlocked.Increment(ref accumulator[accumulatorIndex]);
                var value = accumulator[accumulatorIndex];
                if (value > best)
                {
                    best = value;
                    bestX = accumulatorX;
                    bestY = accumulatorY;
                }
            }
        }

        ArrayPool<int>.Shared.Return(accumulator);
        return new PointS(unchecked((short)(bestX - _r)), unchecked((short)(bestY - _r)));
    }

    internal IEnumerable<PointS> EnumerateEdges(
        short x,
        short y,
        short width,
        short height,
        byte threshold)
    {
        var maxTarget = Math.Max(width, height);

        var xInside = false;
        var yInside = false;

        var returnedPoints = new ConcurrentDictionary<PointS, byte>();

        for (var a = 0; a < maxTarget; a++)
        for (var b = 0; b < maxTarget; b++)
        {
            // Horizontal
            var pointY = unchecked((short) (y + a));
            var pointX = unchecked((short) (x + b));
            if (pointX < width && pointY < height)
            {
                var isFilled = _imageProcessor.GetHighContrast(pointX, pointY, threshold) == byte.MaxValue;
                if (isFilled && !xInside)
                {
                    xInside = true;
                    var point = new PointS(pointX, pointY);
                    if (!returnedPoints.ContainsKey(point))
                    {
                        returnedPoints.TryAdd(point, 0);
                        yield return point;
                    }
                }
                else if (!isFilled && xInside)
                {
                    xInside = false;
                    var point = new PointS(unchecked((short) (pointX - 1)), pointY);
                    if (!returnedPoints.ContainsKey(point))
                    {
                        returnedPoints.TryAdd(point, 0);
                        yield return point;
                    }
                }
            }

            // Vertical
            pointX = unchecked((short) (x + a));
            pointY = unchecked((short) (y + b));
            if (pointX < width && pointY < height)
            {
                var isFilled = _imageProcessor.GetHighContrast(pointX, pointY, threshold) == byte.MaxValue;
                if (isFilled && !yInside)
                {
                    yInside = true;
                    var point = new PointS(pointX, pointY);
                    if (!returnedPoints.ContainsKey(point))
                    {
                        returnedPoints.TryAdd(point, 0);
                        yield return point;
                    }
                }
                else if (!isFilled && yInside)
                {
                    yInside = false;
                    var point = new PointS(pointX, unchecked((short) (pointY - 1)));
                    if (!returnedPoints.ContainsKey(point))
                    {
                        returnedPoints.TryAdd(point, 0);
                        yield return point;
                    }
                }
            }
        }
    }

    private static PointS[] CalculateCirclePoints(
        short r)
    {
        var points = new List<PointS>();
        var rSquared1 = MathF.Pow(r, 2);
        var rSquared2 = MathF.Pow(r - 1, 2);

        for (var y = unchecked((short) -r); y <= r; y++)
        for (var x = unchecked((short) -r); x <= r; x++)
        {
            var distanceSquared = MathF.Pow(x, 2) + MathF.Pow(y, 2);

            if (distanceSquared > rSquared2 && distanceSquared <= rSquared1)
                points.Add(new PointS(x, y));
        }

        return points.ToArray();
    }
}