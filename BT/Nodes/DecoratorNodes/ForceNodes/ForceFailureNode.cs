namespace BT.Nodes.DecoratorNodes.ForceNodes;

internal class ForceFailureNode<TBlackboard> : ForceNode<TBlackboard> where TBlackboard : IBlackboard
{
    public ForceFailureNode(
        string name,
        INode<TBlackboard> child) : base(name, child, NodeStatus.Failure)
    {
    }
}