using BT.Nodes;

namespace BT;

public class BehaviorTree<TBlackboard> : IDisposable where TBlackboard : IBlackboard
{
    private readonly TBlackboard _blackboard;
    private readonly INode<TBlackboard> _rootNode;

    internal List<IDisposable>? Disposables { get; set; }

    public BehaviorTree(
        TBlackboard blackboard,
        INode<TBlackboard> rootNode,
        List<IDisposable>? disposables = null)
    {
        _blackboard = blackboard;
        _rootNode = rootNode;
        Disposables = disposables;
    }

    public NodeStatus TickRoot()
    {
        var tick = new Tick<TBlackboard>(_blackboard);
        return TickRoot(tick);
    }

    internal NodeStatus TickRoot(
        Tick<TBlackboard> tick)
        => _rootNode.Tick(tick);

    public void Halt()
        => _rootNode.Halt();

    public void Dispose()
    {
        if (Disposables is not null)
        {
            foreach (var disposable in Disposables)
                disposable.Dispose();
            
            Disposables = null;
        }
    }
}