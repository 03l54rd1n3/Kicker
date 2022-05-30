using BT.Nodes;

namespace BT.TreeBuilding;

public class ControlNodeBuilder<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly TBlackboard _blackboard;
    private readonly List<IDisposable> _disposables;
    private List<INode<TBlackboard>> _children = new();

    public ControlNodeBuilder(
        TBlackboard blackboard,
        List<IDisposable> disposables)
    {
        _blackboard = blackboard;
        _disposables = disposables;
    }
    
    public ControlNodeBuilder<TBlackboard> With(
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> nodeBuilder)
    {
        var child = nodeBuilder(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        _children.Add(child);
        return this;
    }

    public INode<TBlackboard>[] End()
        => _children.ToArray();
}