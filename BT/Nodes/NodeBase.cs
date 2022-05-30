namespace BT.Nodes;

public abstract class NodeBase<TBlackboard> : INode<TBlackboard> where TBlackboard : IBlackboard
{
    public Guid Id { get; }
    public string Name { get; }

    protected NodeBase(
        string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public abstract NodeStatus Tick(
        Tick<TBlackboard> tick);

    public abstract void Halt();
}