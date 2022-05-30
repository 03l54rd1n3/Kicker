namespace BT.Nodes.DecoratorNodes;

public class RootNode<TBlackboard> : DecoratorNode<TBlackboard> where TBlackboard : IBlackboard
{
    public RootNode(
        string name,
        INode<TBlackboard> child) : base(name, child)
    {
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
        => Child.Tick(tick);
}