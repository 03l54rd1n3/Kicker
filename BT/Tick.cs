namespace BT;

public class Tick<TBlackboard> where TBlackboard : IBlackboard
{
    private Dictionary<string, object> _ports = new();
    public TBlackboard Blackboard { get; }
    public long TimeStamp { get; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public Tick(
        TBlackboard blackboard)
    {
        Blackboard = blackboard;
    }

    public TPort? GetPort<TPort>(
        string key,
        TPort? fallback = default)
        => _ports.TryGetValue(key, out var valueObject)
            ? (TPort) valueObject
            : fallback;

    public void SetPort(
        string key,
        object value)
    {
        if (_ports.TryGetValue(key, out var currentValue))
        {
            if (currentValue.GetType() != value.GetType())
                throw new InvalidOperationException("Port types don't match");
        }

        _ports[key] = value;
    }
}