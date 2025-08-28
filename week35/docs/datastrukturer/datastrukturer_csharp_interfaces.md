# Vanlige interfaces og metoder for datastrukturer i C #

Denne oversikten viser hvilke metoder og interfaces som er typiske for de viktigste datastrukturene i C#.

---

## Array

**Metoder og egenskaper:**

- `Length`
- `GetValue(int index)`
- `SetValue(object value, int index)`
- `CopyTo(Array array, int index)`
- `IndexOf`, `LastIndexOf` (via `Array`-klassen)
- `foreach`-støtte

---

## List<T>

**Interface: IList<T>, ICollection<T>, IEnumerable<T>**

- `Add(T item)`
- `Insert(int index, T item)`
- `Remove(T item)`
- `RemoveAt(int index)`
- `Contains(T item)`
- `IndexOf(T item)`
- `Clear()`
- `Count`
- `this[int index]` (indeksering)
- `Sort()`
- `Reverse()`
- `foreach`-støtte


## Stack<T>

**Interface: IEnumerable<T>, ICollection**

- `Push(T item)`
- `Pop()`
- `Peek()`
- `TryPop(out T result)`
- `TryPeek(out T result)`
- `Contains(T item)`
- `Clear()`
- `Count`
- `foreach`-støtte

---

## Queue<T>

**Interface: IEnumerable<T>, ICollection**

- `Enqueue(T item)`
- `Dequeue()`
- `Peek()`
- `TryDequeue(out T result)`
- `TryPeek(out T result)`
- `Contains(T item)`
- `Clear()`
- `Count`
- `foreach`-støtte

---

## LinkedList<T>

**Interface: IEnumerable<T>, ICollection**

- `AddFirst(T item)`
- `AddLast(T item)`
- `AddAfter(LinkedListNode<T> node, T item)`
- `AddBefore(LinkedListNode<T> node, T item)`
- `Remove(T item)`
- `RemoveFirst()`
- `RemoveLast()`
- `Find(T item)`
- `Clear()`
- `Count`
- `First`, `Last`
- `foreach`-støtte

---

## Tree (SortedSet<T>, SortedDictionary<TKey,TValue>)

**Interface: ICollection<T>, IEnumerable<T>**

- `Add(T item)`
- `Remove(T item)`
- `Contains(T item)`
- `Clear()`
- `Count`
- `Min`, `Max` (SortedSet)
- `Keys`, `Values` (SortedDictionary)
- `foreach`-støtte

---
<div style="page-break-after:always;"></div>

## Heap (PriorityQueue<TElement,TPriority>)

**Interface: IEnumerable<TElement>**

- `Enqueue(TElement element, TPriority priority)`
- `Dequeue()`
- `Peek()`
- `TryDequeue(out TElement element, out TPriority priority)`
- `Count`
- `Clear()`
- `foreach`-støtte

---

## Graph (Dictionary<T, List<T>> eller egen klasse)

**Interface: IDictionary<T, List<T>>, IEnumerable<KeyValuePair<T, List<T>>>**

- `Add(T node, List<T> neighbors)`
- `Remove(T node)`
- `ContainsKey(T node)`
- `TryGetValue(T node, out List<T> neighbors)`
- `Keys`, `Values`
- `foreach`-støtte

---

Denne listen gir et raskt overblikk over hvilke metoder og egenskaper du kan forvente å bruke med de ulike datastrukturene i C#.
