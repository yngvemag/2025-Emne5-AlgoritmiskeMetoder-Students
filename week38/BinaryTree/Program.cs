using BinaryTree.Datastructures;

var tree = new BinarySearchTree<int>();

// Legg inn noen verdier (duplikater går til høyre etter konvensjon).
tree.Add(10);
tree.Add(5);
tree.Add(7);
tree.Add(4);
tree.Add(15);
tree.Add(12); 
tree.Add(17);


Console.WriteLine($"Count: {tree.Count}");         // 6
Console.WriteLine($"Min:   {tree.Min()}");          // 4
Console.WriteLine($"Max:   {tree.Max()}");          // 75
Console.WriteLine($"Height:{tree.Height()}");       // avhenger av innsettingsrekkefølge

// In-Order gir stigende rekkefølge
Console.Write("InOrder:  ");
foreach (var x in tree.AsEnumerable(TraversalOrder.InOrder)) // bruker IEnumerable<T>-støtten (in-order)
    Console.Write($"{x} ");
Console.WriteLine();

Console.Write("LevelOrder:  ");
foreach (var x in tree.AsEnumerable(TraversalOrder.LevelOrder)) // bruker IEnumerable<T>-støtten (in-order)
    Console.Write($"{x} ");
Console.WriteLine();

// Sjekk om verdi finnes
Console.WriteLine($"Contains 12? {tree.Contains(12)}"); // true
Console.WriteLine($"Contains 99? {tree.Contains(99)}"); // false

// Fjern en verdi (demonstrerer de tre tilfellene automatisk)
Console.WriteLine($"Remove 15: {tree.Remove(15)}");     // true
Console.WriteLine($"Remove 99: {tree.Remove(99)}");     // false

Console.Write("InOrder after remove: ");
foreach (var x in tree)
    Console.Write($"{x} ");
Console.WriteLine();

Console.Write("PreOrder: ");
foreach (var x in tree.AsEnumerable(TraversalOrder.PreOrder))
    Console.Write($"{x} ");
Console.WriteLine();

Console.Write("PostOrder: ");
foreach (var x in tree.AsEnumerable(TraversalOrder.PostOrder))
    Console.Write($"{x} ");
Console.WriteLine();

// Tøm treet
tree.Clear();
Console.WriteLine($"Cleared. IsEmpty = {tree.IsEmpty}, Count = {tree.Count}");