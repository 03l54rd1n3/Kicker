namespace BT.Nodes.ActionNodes;

internal class SimpleActionNode<TBlackboard> : ActionNode<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly Func<Tick<TBlackboard>, NodeStatus> _func;

    public SimpleActionNode(
        string name,
        Func<Tick<TBlackboard>, NodeStatus> func) : base(name)
    {
        _func = func;
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
        => _func(tick);

    public override void Halt()
    {
        
    }
}