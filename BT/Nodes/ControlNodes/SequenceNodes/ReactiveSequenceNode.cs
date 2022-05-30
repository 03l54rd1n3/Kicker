namespace BT.Nodes.ControlNodes.SequenceNodes;

internal class ReactiveSequenceNode<TBlackboard> : ControlNode<TBlackboard> where TBlackboard : IBlackboard
{
    public ReactiveSequenceNode(
        string name,
        INode<TBlackboard>[] children) : base(name, children)
    {
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        foreach (var child in _children)
        {
            var childStatus = child.Tick(tick);
            switch (childStatus)
            {
                case NodeStatus.Running:
                    return childStatus;
                case NodeStatus.Failure:
                    HaltAllChildren();
                    return childStatus;
            }
        }

        HaltAllChildren();
        return NodeStatus.Success;
    }
}