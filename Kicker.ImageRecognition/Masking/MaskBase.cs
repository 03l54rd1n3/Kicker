namespace Kicker.ImageRecognition.Masking;

public abstract class MaskBase : IMask
{
    protected HashSet<int>? _containedPoints = new();

    public bool IsCalculated { get; private set; }

    public virtual bool Contains(
        short x,
        short y)
    {
        if (IsCalculated)
            return _containedPoints!.Contains(x * 10000 + y);

        return ContainsInternal(x, y);
    }

    public virtual void Calculate(
        short width,
        short height)
    {
        if (IsCalculated)
            throw new InvalidOperationException("Already calculated");

        foreach (var (x, y) in GetPossiblePoints(width, height))
        {
            if (Contains(x, y))
                _containedPoints!.Add(x * 10000 + y);
        }

        IsCalculated = true;
    }

    protected abstract bool ContainsInternal(
        short x,
        short y);

    protected abstract IEnumerable<(short X, short Y)> GetPossiblePoints(
        short width,
        short height);

    protected virtual void Dispose(
        bool disposing)
    {
        if (disposing)
        {
            _containedPoints = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}