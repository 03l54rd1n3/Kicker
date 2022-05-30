namespace BT.Nodes.ConditionNodes;

internal class SimpleConditionNode<TBlackboard> : NodeBase<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly Func<Tick<TBlackboard>, bool> _conditionFunc;

    public SimpleConditionNode(
        string name,
        Func<Tick<TBlackboard>, bool> conditionFunc) : base(name)
    {
        _conditionFunc = conditionFunc;
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
        => _conditionFunc(tick) ? NodeStatus.Success : NodeStatus.Failure;

    public override void Halt()
    {
    }
}