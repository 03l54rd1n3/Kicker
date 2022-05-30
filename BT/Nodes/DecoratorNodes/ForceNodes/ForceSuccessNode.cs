namespace BT.Nodes.DecoratorNodes.ForceNodes;

internal class ForceSuccessNode<TBlackboard> : ForceNode<TBlackboard> where TBlackboard : IBlackboard
{
    public ForceSuccessNode(
        string name,
        INode<TBlackboard> child) : base(name, child, NodeStatus.Success)
    {
    }
}