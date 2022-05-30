using BT.Nodes;
using BT.Nodes.DecoratorNodes;

namespace BT.TreeBuilding;

public class BehaviorTreeBuilder<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly TBlackboard _blackboard;
    private readonly List<IDisposable> _disposables = new();
    private INode<TBlackboard>? _rootNode;

    public BehaviorTreeBuilder(
        TBlackboard blackboard)
    {
        _blackboard = blackboard;
    }

    public BehaviorTreeBuilder<TBlackboard> With(
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> firstNodeBuilder)
        => With("RootNote", firstNodeBuilder);

    internal BehaviorTreeBuilder<TBlackboard> With(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> firstNodeBuilder)
    {
        var firstNode = firstNodeBuilder(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        _rootNode = new RootNode<TBlackboard>(name, firstNode);
        return this;
    }

    public BehaviorTree<TBlackboard> Build()
    {
        if (_rootNode is null)
            throw new InvalidOperationException("RootNote not initialized");

        var behaviorTree = new BehaviorTree<TBlackboard>(_blackboard, _rootNode, _disposables);
        return behaviorTree;
    }
}