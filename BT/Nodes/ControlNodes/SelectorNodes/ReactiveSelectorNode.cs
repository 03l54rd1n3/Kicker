namespace BT.Nodes.ControlNodes.SelectorNodes;

internal class ReactiveSelectorNode<TBlackboard> : ControlNode<TBlackboard> where TBlackboard : IBlackboard
{
    public ReactiveSelectorNode(
        string name,
        INode<TBlackboard>[] children) : base(name, children)
    {
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        for (var i = 0; i < _children.Length; i++)
        {
            var childStatus = _children[i].Tick(tick);
            switch (childStatus)
            {
                case NodeStatus.Running:
                {
                    for (var j = i + 1; j < _children.Length; j++)
                        _children[j].Halt();
                
                    return childStatus;
                }
                case NodeStatus.Success:
                    HaltAllChildren();
                    return childStatus;
            }
        }

        HaltAllChildren();
        return NodeStatus.Success;
    }
}