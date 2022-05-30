namespace BT.Nodes.DecoratorNodes.ForceNodes;

internal abstract class ForceNode <TBlackboard> : DecoratorNode<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly NodeStatus _forceStatus;

    protected ForceNode(
        string name,
        INode<TBlackboard> child,
        NodeStatus forceStatus) : base(name, child)
    {
        _forceStatus = forceStatus;
    }
    
    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        var childStatus = Child.Tick(tick);
        return childStatus == NodeStatus.Running ? childStatus : _forceStatus;
    }
}