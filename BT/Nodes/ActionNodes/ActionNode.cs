namespace BT.Nodes.ActionNodes;

public abstract class ActionNode<TBlackboard> : NodeBase<TBlackboard> where TBlackboard : IBlackboard
{
    internal ActionNode(
        string name) : base(name)
    {
    }
}