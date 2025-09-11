
using BinaryTree.Datastructures;

BinarySearchTree<int> myTree = new();
myTree.Add(45);
myTree.Add(34);
myTree.Add(71);
myTree.Add(22);
myTree.Add(87);
myTree.Add(66);
myTree.Add(40);

Console.WriteLine($"Antall noder i treet: {myTree.Count}");

Console.WriteLine($"Inneholder treet 40? {myTree.Contains(40)}");
Console.WriteLine($"Inneholder treet 55? {myTree.Contains(55)}");