namespace BT.Nodes.DecoratorNodes.RepetitiveNodes;

internal class RepeatNode<TBlackboard> : RepetitiveNode<TBlackboard> where TBlackboard : IBlackboard
{
    public RepeatNode(
        string name,
        INode<TBlackboard> child,
        string n) : base(name, child, NodeStatus.Failure, n)
    {
    }
}