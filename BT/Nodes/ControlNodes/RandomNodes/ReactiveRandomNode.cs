namespace BT.Nodes.ControlNodes.RandomNodes;

internal class ReactiveRandomNode<TBlackboard> : ControlNode<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly Random _random;
    
    public ReactiveRandomNode(
        string name,
        INode<TBlackboard>[] children,
        Random? random = null) : base(name, children)
    {
        _random = random ?? new Random();
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        HaltAllChildren();
        var index = _random.Next(_children.Length);
        return _children[index].Tick(tick);
    }
}