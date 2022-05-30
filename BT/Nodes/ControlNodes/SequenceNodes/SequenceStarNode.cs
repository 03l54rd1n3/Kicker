namespace BT.Nodes.ControlNodes.SequenceNodes;

internal class SequenceStarNode<TBlackboard> : ControlNode<TBlackboard> where TBlackboard : IBlackboard
{
    private int _index;
    
    public SequenceStarNode(
        string name,
        INode<TBlackboard>[] children) : base(name, children)
    {
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        for (; _index < _children.Length; _index++)
        {
            var childStatus = _children[_index].Tick(tick);

            switch (childStatus)
            {
                case NodeStatus.Running:
                case NodeStatus.Failure:
                    return childStatus;
            }
        }

        HaltAllChildren();
        return NodeStatus.Success;
    }

    protected override void HaltAllChildren()
    {
        base.HaltAllChildren();
        _index = 0;
    }
}