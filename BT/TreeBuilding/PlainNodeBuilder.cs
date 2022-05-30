using BT.Nodes;
using BT.Nodes.ActionNodes;
using BT.Nodes.ConditionNodes;
using BT.Nodes.ControlNodes.RandomNodes;
using BT.Nodes.ControlNodes.SelectorNodes;
using BT.Nodes.ControlNodes.SequenceNodes;
using BT.Nodes.DecoratorNodes;
using BT.Nodes.DecoratorNodes.ForceNodes;
using BT.Nodes.DecoratorNodes.RepetitiveNodes;

namespace BT.TreeBuilding;

public class PlainNodeBuilder<TBlackboard> where TBlackboard : IBlackboard
{
    private readonly TBlackboard _blackboard;
    private readonly List<IDisposable> _disposables;

    public PlainNodeBuilder(
        TBlackboard blackboard,
        List<IDisposable> disposables)
    {
        _blackboard = blackboard;
        _disposables = disposables;
    }

    public INode<TBlackboard> Sequence(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new SequenceNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> ReactiveSequence(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new ReactiveSequenceNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> SequenceStar(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new SequenceStarNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> Selector(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new SelectorNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> ReactiveSelector(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new ReactiveSelectorNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> Random(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new RandomNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> ReactiveRandom(
        string name,
        Func<ControlNodeBuilder<TBlackboard>, INode<TBlackboard>[]> childrenFunc)
    {
        var children = childrenFunc(new ControlNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new ReactiveRandomNode<TBlackboard>(name, children);
    }
    
    public INode<TBlackboard> Inverter(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> childFunc)
    {
        var child = childFunc(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new InverterNode<TBlackboard>(name, child);
    }
    
    public INode<TBlackboard> ForceSuccess(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> childFunc)
    {
        var child = childFunc(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new ForceSuccessNode<TBlackboard>(name, child);
    }
    
    public INode<TBlackboard> ForceFailure(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> childFunc)
    {
        var child = childFunc(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new ForceFailureNode<TBlackboard>(name, child);
    }
    
    public INode<TBlackboard> Repeat(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> childFunc,
        string n)
    {
        var child = childFunc(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new RepeatNode<TBlackboard>(name, child, n);
    }
    
    public INode<TBlackboard> Retry(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> childFunc,
        string n)
    {
        var child = childFunc(new PlainNodeBuilder<TBlackboard>(_blackboard, _disposables));
        return new RetryNode<TBlackboard>(name, child, n);
    }
    
    public INode<TBlackboard> SimpleCondition(
        string name,
        Func<Tick<TBlackboard>, bool> conditionFunc)
        => new SimpleConditionNode<TBlackboard>(name, conditionFunc);
    
    public INode<TBlackboard> SimpleAction(
        string name,
        Func<Tick<TBlackboard>, NodeStatus> actionFunc)
        => new SimpleActionNode<TBlackboard>(name, actionFunc);

    public INode<TBlackboard> Action<TActionNode>(
        string name)
        where TActionNode : ActionNode<TBlackboard>
        => (TActionNode) (Activator.CreateInstance(typeof(TActionNode), name) ?? throw new InvalidOperationException());

    public INode<TBlackboard> Action<TActionNode>(
        string name,
        Func<TActionNode> actionFunc)
        where TActionNode : ActionNode<TBlackboard>
        => actionFunc();

    public INode<TBlackboard> AsyncAction(
        string name,
        Func<Tick<TBlackboard>, CancellationToken, Task> taskFunc)
    {
        var asyncActionNode = new AsyncActionNode<TBlackboard>(name, taskFunc);
        _disposables.Add(asyncActionNode);
        return asyncActionNode;
    }
    
    public INode<TBlackboard> SubTree(
        string name,
        Func<PlainNodeBuilder<TBlackboard>, INode<TBlackboard>> childFunc)
    {
        var behaviorTreeBuilder = new BehaviorTreeBuilder<TBlackboard>(_blackboard);
        var behaviorTree = behaviorTreeBuilder
            .With(name, childFunc)
            .Build();
        
        var subTreeNode = new SubTreeNode<TBlackboard>(name, behaviorTree);
        
        _disposables.Add(subTreeNode);
        return subTreeNode;
    }
}