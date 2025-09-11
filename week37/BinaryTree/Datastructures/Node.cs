namespace BinaryTree.Datastructures;

public sealed class Node<T>(T value)
{
    public T Value { get; init; } = value;
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }
}