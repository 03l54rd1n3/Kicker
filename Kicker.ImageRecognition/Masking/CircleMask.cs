namespace Kicker.ImageRecognition.Masking;

public class CircleMask : MaskBase
{
    private readonly int _x;
    private readonly int _y;
    private readonly int _r;
    private readonly float _rSquared;

    public CircleMask(
        int x,
        int y,
        int r)
    {
        _x = x;
        _y = y;
        _r = r;
        _rSquared = MathF.Pow(r, 2);
    }

    protected override bool ContainsInternal(
        int x,
        int y)
    {
        var distanceSquared = MathF.Pow(_x - x, 2) + MathF.Pow(_y - y, 2);
        return distanceSquared <= _rSquared;
    }

    protected override IEnumerable<(int X, int Y)> GetPossiblePoints(
        int width,
        int height)
    {
        var startX = Math.Max(_x - _r, 0);
        var endX = Math.Min(_x + _r, width);

        var startY = Math.Max(_y - _r, 0);
        var endY = Math.Min(_y + _r, height);

        for (var y = startY; y < endY; y++)
        for (var x = startX; x < endX; x++)
            yield return (x, y);
    }
}