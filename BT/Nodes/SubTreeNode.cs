namespace BT.Nodes;

public class SubTreeNode<TBlackboard> : NodeBase<TBlackboard>, IDisposable where TBlackboard : IBlackboard
{
    private readonly BehaviorTree<TBlackboard> _subTree;

    public SubTreeNode(
        string name,
        BehaviorTree<TBlackboard> subTree) : base(name)
    {
        _subTree = subTree;
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
        => _subTree.TickRoot(tick);

    public override void Halt()
        => _subTree.Halt();

    public void Dispose()
    {
        _subTree.Dispose();
    }
}