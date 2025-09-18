namespace BinaryTree.Datastructures;

public sealed class Node<T>(T value)
{
    public T Value { get; set; } = value;
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }
}