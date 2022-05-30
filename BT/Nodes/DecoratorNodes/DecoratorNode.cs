namespace BT.Nodes.DecoratorNodes;

public abstract class DecoratorNode<TBlackboard> : NodeBase<TBlackboard> where TBlackboard : IBlackboard
{
    public INode<TBlackboard> Child { get; }

    protected DecoratorNode(
        string name,
        INode<TBlackboard> child)
        : base(name)
    {
        Child = child;
    }

    public override void Halt() => Child.Halt();
}
