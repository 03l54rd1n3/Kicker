namespace BT.Nodes.DecoratorNodes.RepetitiveNodes;

internal abstract class RepetitiveNode <TBlackboard> : DecoratorNode<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly NodeStatus _exitStatus;
    private readonly string _n;
    private int _count;
    private bool _halted;

    protected RepetitiveNode(
        string name,
        INode<TBlackboard> child,
        NodeStatus exitStatus,
        string n) : base(name, child)
    {
        _n = n;
        _exitStatus = exitStatus;
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        if (!int.TryParse(_n, out var n))
            n = tick.GetPort<int>(_n);
            
        var childStatus = NodeStatus.Failure;
        for (; _count < n; _count++)
        {
            if (_halted)
            {
                _count = 0;
                _halted = false;
                return NodeStatus.Failure;
            }

            childStatus = Child.Tick(tick);

            if (childStatus == NodeStatus.Running)
                return childStatus;
            if (childStatus == _exitStatus)
                break;
        }

        _count = 0;
        return childStatus;
    }

    public override void Halt()
    {
        _count = 0;
        _halted = true;
        base.Halt();
    }
}