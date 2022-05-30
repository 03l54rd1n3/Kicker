namespace BT.Nodes.ActionNodes;

internal class AsyncActionNode<TBlackboard> : ActionNode<TBlackboard>, IDisposable where TBlackboard : IBlackboard 
{
    private readonly Func<Tick<TBlackboard>, CancellationToken, Task> _taskFunc;
    private CancellationTokenSource _cts = new();
    protected Task? _task;

    public AsyncActionNode(
        string name,
        Func<Tick<TBlackboard>, CancellationToken, Task> taskFunc) : base(name)
    {
        _taskFunc = taskFunc;
    }

    public override NodeStatus Tick(
        Tick<TBlackboard> tick)
    {
        if (_task is null)
        {
            if (_cts.IsCancellationRequested && !_cts.TryReset())
                return NodeStatus.Failure;
            
            _task = _taskFunc(tick, _cts.Token);
        }

        if (_task.IsCompletedSuccessfully)
        {
            _task = null;
            return NodeStatus.Success;
        }

        if (_task.IsFaulted || _task.IsCanceled)
        {
            _task = null;
            return NodeStatus.Failure;
        }

        return NodeStatus.Running;
    }

    public override void Halt()
    {
        _cts.Cancel();
        _task = null;
    }

    public void Dispose()
    {
        _task?.Dispose();
        _cts.Dispose();
    }
}