namespace BT.Nodes.ControlNodes;

public abstract class ControlNode <TBlackboard> : NodeBase<TBlackboard> where TBlackboard : IBlackboard
{
    public IReadOnlyCollection<INode<TBlackboard>> Children => _children;
    protected INode<TBlackboard>[] _children;

    protected ControlNode(
        string name,
        INode<TBlackboard>[] children)
        : base(name)
    {
        _children = children;
    }

    public override void Halt() => HaltAllChildren();

    protected virtual void HaltAllChildren()
    {
        foreach (var child in _children)
            child.Halt();
    }
}