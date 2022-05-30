namespace BT.Nodes.DecoratorNodes.RepetitiveNodes;

internal class RetryNode<TBlackboard> : RepetitiveNode<TBlackboard> where TBlackboard : IBlackboard
{
    public RetryNode(
        string name,
        INode<TBlackboard> child,
        string n) : base(name, child, NodeStatus.Success, n)
    {
    }
}
