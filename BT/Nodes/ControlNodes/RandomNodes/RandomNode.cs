namespace BT.Nodes.ControlNodes.RandomNodes;

internal class RandomNode<TBlackboard> : ControlNode<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly Random _random;
    private int _index = -1;
    
    public RandomNode(
        string name,
        INode<TBlackboard>[] children,
        Random? random = null) : base(name, children)
    {
        _random = random ?? new Random();
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        if (_index == -1)
            _index = _random.Next(_children.Length);

        var childStatus = _children[_index].Tick(tick);
        if (childStatus != NodeStatus.Running)
            _index = -1;
        
        return childStatus;
    }

    public override void Halt()
    {
        base.Halt();
        _index = 0;
    }
}