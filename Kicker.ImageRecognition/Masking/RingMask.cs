namespace Kicker.ImageRecognition.Masking;

public class RingMask : MaskBase
{
    private readonly int _x;
    private readonly int _y;
    private readonly int _outerR;
    private readonly float _innerRSquared;
    private readonly float _outerRSquared;

    public RingMask(
        int x,
        int y,
        int innerR,
        int outerR)
    {
        _x = x;
        _y = y;
        _outerR = outerR;
        _innerRSquared = MathF.Pow(innerR, 2);
        _outerRSquared = MathF.Pow(outerR, 2);
    }

    protected override bool ContainsInternal(
        int x,
        int y)
    {
        var distanceSquared = MathF.Pow(_x - x, 2) + MathF.Pow(_y - y, 2);
        return distanceSquared >= _innerRSquared && distanceSquared <= _outerRSquared;
    }

    protected override IEnumerable<(int X, int Y)> GetPossiblePoints(
        int width,
        int height)
    {
        var startX = Math.Max(_x - _outerR, 0);
        var endX = Math.Min(_x + _outerR, width);

        var startY = Math.Max(_y - _outerR, 0);
        var endY = Math.Min(_y + _outerR, height);

        for (var y = startY; y < endY; y++)
        for (var x = startX; x < endX; x++)
            yield return (x, y);
    }
}