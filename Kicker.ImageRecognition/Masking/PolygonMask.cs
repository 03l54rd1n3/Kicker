using System.Drawing;
using Kicker.Shared;

namespace Kicker.ImageRecognition.Masking;

public class PolygonMask : MaskBase
{
    private readonly int _imageWidth;
    private readonly Point[] _points;

    public PolygonMask(
        int imageWidth,
        params Point[] points)
    {
        if (points.Length < 2)
            throw new ArgumentException("Invalid number of points", nameof(points));

        _imageWidth = imageWidth;
        _points = points.Append(points[0]).ToArray();
    }

    protected override bool ContainsInternal(
        int x,
        int y)
    {
        var pointLineStart = new Point(x, y);
        var pointLineEnd = new Point(_imageWidth, y);
        var countOfIntersects = 0;

        for (var i = 1; i < _points.Length; i++)
        {
            var startPoint = _points[i - 1];
            if (startPoint.Y == y)
                startPoint.Y++;

            var endPoint = _points[i];
            if (endPoint.Y == y)
                endPoint.Y++;

            var doIntersect = Geometry.DoIntersect(pointLineStart, pointLineEnd, startPoint, endPoint);
            if (doIntersect)
                countOfIntersects++;
        }

        return countOfIntersects % 2 == 1;
    }

    protected override IEnumerable<(int X, int Y)> GetPossiblePoints(
        int width,
        int height)
    {
        var startX = _points.Min(_ => _.X);
        var endX = _points.Max(_ => _.X);

        var startY = _points.Min(_ => _.Y);
        var endY = _points.Max(_ => _.Y);

        for (var y = startY; y < endY; y++)
        for (var x = startX; x < endX; x++)
            yield return (x, y);
    }
}