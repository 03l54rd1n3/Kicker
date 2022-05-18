namespace Kicker.ImageRecognition.Masking;

public class RingMask : MaskBase
{
    private readonly short _x;
    private readonly short _y;
    private readonly short _outerR;
    private readonly float _innerRSquared;
    private readonly float _outerRSquared;

    public RingMask(
        short x,
        short y,
        short innerR,
        short outerR)
    {
        _x = x;
        _y = y;
        _outerR = outerR;
        _innerRSquared = MathF.Pow(innerR, 2);
        _outerRSquared = MathF.Pow(outerR, 2);
    }

    protected override bool ContainsInternal(
        short x,
        short y)
    {
        var distanceSquared = MathF.Pow(_x - x, 2) + MathF.Pow(_y - y, 2);
        return distanceSquared >= _innerRSquared && distanceSquared <= _outerRSquared;
    }

    protected override IEnumerable<(short X, short Y)> GetPossiblePoints(
        short width,
        short height)
    {
        var startX = unchecked((short)Math.Max(_x - _outerR, 0));
        var endX = unchecked((short)Math.Min(_x + _outerR, width));

        var startY = unchecked((short)Math.Max(_y - _outerR, 0));
        var endY = unchecked((short)Math.Min(_y + _outerR, height));

        for (var y = startY; y < endY; y++)
        for (var x = startX; x < endX; x++)
            yield return (x, y);
    }
}