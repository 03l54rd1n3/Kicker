namespace BT.Nodes;

public interface INode<TBlackboard> where TBlackboard : IBlackboard
{
    Guid Id { get; }

    string Name { get; }

    NodeStatus Tick(
        Tick<TBlackboard> tick);

    void Halt();
}