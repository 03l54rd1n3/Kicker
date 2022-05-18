namespace Kicker.Shared;

[Serializable]
public struct PointS : IEquatable<PointS>
{
    private short _x;
    private short _y;

    public static readonly PointS Empty;

    public short X
    {
        readonly get => _x;
        set => _x = value;
    }

    public short Y
    {
        readonly get => _y;
        set => _y = value;
    }

    public PointS(
        short x = 0,
        short y = 0)
    {
        _x = x;
        _y = y;
    }

    public readonly override string ToString()
        => $"{{X={X},Y={Y}}}";

    public bool Equals(
        PointS other)
    {
        if (_x == other._x && _y == other._y)
            return true;
        return false;
    }

    public override bool Equals(
        object? obj)
        => obj is PointS other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(_x, _y);

    public static bool operator ==(
        PointS left,
        PointS right)
        => left.Equals(right);

    public static bool operator !=(
        PointS left,
        PointS right)
        => !left.Equals(right);
}