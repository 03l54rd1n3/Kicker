namespace Kicker.ImageRecognition.Masking;

public class CircleMask : MaskBase
{
    private readonly short _x;
    private readonly short _y;
    private readonly short _r;
    private readonly float _rSquared;

    public CircleMask(
        short x,
        short y,
        short r)
    {
        _x = x;
        _y = y;
        _r = r;
        _rSquared = MathF.Pow(r, 2);
    }

    protected override bool ContainsInternal(
        short x,
        short y)
    {
        var distanceSquared = MathF.Pow(_x - x, 2) + MathF.Pow(_y - y, 2);
        return distanceSquared <= _rSquared;
    }

    protected override IEnumerable<(short X, short Y)> GetPossiblePoints(
        short width,
        short height)
    {
        var startX = unchecked((short) Math.Max(_x - _r, 0));
        var endX = unchecked((short) Math.Min(_x + _r, width));

        var startY = unchecked((short) Math.Max(_y - _r, 0));
        var endY = unchecked((short) Math.Min(_y + _r, height));

        for (var y = startY; y < endY; y++)
        for (var x = startX; x < endX; x++)
            yield return (x, y);
    }
}