namespace BT.Nodes.DecoratorNodes;

internal class InverterNode<TBlackboard> : DecoratorNode<TBlackboard> where TBlackboard : IBlackboard
{
    public InverterNode(
        string name,
        INode<TBlackboard> child) : base(name, child)
    {
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        var childStatus = Child.Tick(tick);
        
        if (childStatus == NodeStatus.Running)
            return childStatus;

        return childStatus == NodeStatus.Success ? NodeStatus.Failure : NodeStatus.Success;
    }
}