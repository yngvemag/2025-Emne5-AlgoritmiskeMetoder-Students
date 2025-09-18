using System.Collections;

namespace BinaryTree.Datastructures;


public enum TraversalOrder
{
    InOrder,
    PreOrder,
    PostOrder,
    LevelOrder
}

public class BinarySearchTree<T>(IComparer<T>? comparer = null) : IEnumerable<T>
    where T : IComparable<T>
{
    private readonly IComparer<T> _comparer = comparer ?? Comparer<T>.Default;
    private Node<T>? _root = null;
    private int _count;


    public int Count => _count;
    public bool IsEmpty => _count == 0;

    public void Add(T value)
    {
        if (_root == null)
        {
            _root = new Node<T>(value);
            _count++;
            return;
        }

        // start from the root
        Node<T> current = _root;
        while (true)
        {
            int cmp = _comparer.Compare(value, current.Value);

            // go left
            if (cmp < 0)
            {
                if (current.Left == null)
                {
                    // vi kan legge til noden her
                    current.Left = new Node<T>(value);
                    _count++;
                    return;
                }
                current = current.Left; // fortsett nedover til venstre
            }
            else // go right
            {
                if (current.Right == null)
                {
                    // vi kan legge til noden her
                    current.Right = new Node<T>(value);
                    _count++;
                    return;
                }
                current = current.Right; // fortsett nedover til høyre
            }
        }
    }

    public bool Contains(T value)
    {
        Node<T>? current = _root;
        while (current != null) // så lenge det finnes noder å sjekke
        {
            int cmp = _comparer.Compare(value, current.Value);
            if (cmp == 0) return true; // vi fant verdien

            // gå venstre eller høyre avhengig av sammenligningen
            current = (cmp < 0)
                ? current.Left
                : current.Right;
        }

        return false;
    }

    public bool Remove(T value)
    {
        Node<T>? current = _root;
        Node<T>? parent = null;

        // 1) Finn noden som skal fjernes + hold på forelder (node, parent)
        while (current != null)
        {
            int cmp = _comparer.Compare(value, current.Value);
            if (cmp == 0) break;

            parent = current; // hold på forelder
            current = (cmp < 0) // gå venstre eller høyre
                ? current.Left
                : current.Right;
        }

        if (current == null) return false; // ikke funnet

        // 2) Hvis noden har to barn: finn etterfølger og bytt verdier
        // if (current is { Left: not null, Right: not null })
        if (current.Left != null && current.Right != null)
        {
            // Finn venstre-mest node i høyre subtre (minst i høyre gren)
            Node<T> succParent = current;
            Node<T> succ = current.Right;
            while (succ!.Left != null)
            {
                succParent = succ;
                succ = succ.Left;
            }

            // Kopier etterfølgerens verdi inn i current
            current.Value = succ.Value;

            // Videre: vi skal fjerne "succ"-noden, som har maks ett barn
            current = succ;
            parent = succParent;
        }

        // 3) Nå har 'current' maks ett barn
        Node<T>? replacement = current.Left ?? current.Right; // kan være null (blad)


        if (parent == null)
        {
            // Fjerner roten
            _root = replacement;
        }
        else if (parent.Left == current)
        {
            parent.Left = replacement;
        }
        else
        {
            parent.Right = replacement;
        }

        _count--;
        return true;
    }

    /// <summary>
    /// Tømmer treet (O(1) for pekere, GC rydder minnet).
    /// </summary>
    public void Clear()
    {
        _root = null;
        _count = 0;
    }

    /// <summary>
    /// Høyden på treet (antall kanter i lengste vei fra rot til blad). Tomt tre = -1, ett element = 0.
    /// </summary>
    public int Height() => Height(_root);

    private static int Height(Node<T>? node)
    {
        // Rekursiv definisjon: høyde = 1 + maks(høyre, venstre)
        if (node == null) return -1;

        // rekursivt kall
        return 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    /// <summary>
    /// Returnerer minste verdi i treet (kaster hvis tomt).
    /// </summary>
    public T Min()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        Node<T> n = _root;
        while (n.Left != null)
            n = n.Left;

        return n.Value;
    }

    /// <summary>
    /// Returnerer største verdi i treet (kaster hvis tomt).
    /// </summary>
    public T Max()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        Node<T> n = _root;
        while (n.Right != null)
            n = n.Right;
        return n.Value;
    }

    public IEnumerable<T> AsEnumerable(TraversalOrder order = TraversalOrder.InOrder)
    {
        return order switch
        {
            TraversalOrder.InOrder => InOrderEnumerable(),
            TraversalOrder.PreOrder => PreOrderEnumerable(),
            TraversalOrder.PostOrder => PostOrderEnumerable(),
            TraversalOrder.LevelOrder => LevelOrderEnumerable(),
            _ => InOrderEnumerable()
        };
    }

    // venstre, rot, høyre
    // Besøk venstre subtre først, deretter roten, og til slutt høyre subtre
    // Brukes i binære søketrær (BST) for å hente elementer i sortert rekkefølge
    private IEnumerable<T> InOrderEnumerable()
    {
        var stack = new Stack<Node<T>>();
        Node<T>? current = _root;

        while (stack.Count > 0 || current != null)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }
            var node = stack.Pop();
            yield return node.Value;
            current = node.Right;
        }
    }

    // root, venstre, høyre
    // Besøk roten først, deretter venstre subtre, og til slutt høyre subtre
    // Brukes når du ønsker å prosessere roten før barna
    // Vanlig ved serialisering (lagring) av treet eller ved kopiering av strukturen
    private IEnumerable<T> PreOrderEnumerable()
    {
        if (_root == null) yield break;
        var stack = new Stack<Node<T>>();
        stack.Push(_root);

        while (stack.Count > 0)
        {
            var n = stack.Pop();
            yield return n.Value;

            if (n.Right != null) stack.Push(n.Right);
            if (n.Left != null) stack.Push(n.Left);
        }
    }

    // venstre, høyre, root
    // Besøk barna før roten.
    // Brukes når man skal slette et tre eller gjøre beregninger der barna må prosesseres før foreldrene
    // Vanlig i uttrykks-trær (Expression Trees) for å evaluere uttrykk
    private IEnumerable<T> PostOrderEnumerable()
    {
        if (_root == null) yield break;

        // To-stack-løsning
        var s1 = new Stack<Node<T>>();
        var s2 = new Stack<Node<T>>();
        s1.Push(_root);

        while (s1.Count > 0)
        {
            var n = s1.Pop();
            s2.Push(n);
            if (n.Left != null) s1.Push(n.Left);
            if (n.Right != null) s1.Push(n.Right);
        }

        while (s2.Count > 0)
            yield return s2.Pop().Value;
    }

    // Breadth-First Search (BFS) besøker nodene nivå for nivå, fra toppen og nedover
    // Bruker en kø (FIFO) for å holde orden på hvilke noder som skal besøkes
    // Veldig nyttig i situasjoner der du trenger å jobbe med elementer etter "avstand" fra roten, som i navigasjon eller spillutvikling
    private IEnumerable<T> LevelOrderEnumerable()
    {
        if (_root == null) yield break;

        var q = new Queue<Node<T>>();
        q.Enqueue(_root);

        while (q.Count > 0)
        {
            var n = q.Dequeue();
            yield return n.Value;

            if (n.Left != null) q.Enqueue(n.Left);
            if (n.Right != null) q.Enqueue(n.Right);
        }
    }

    /// <summary>
    /// Standard-iterator (foreach) returnerer in-order.
    /// </summary>
    public IEnumerator<T> GetEnumerator() => AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}