using System.Collections;

namespace CustomListImplementation;

public class GaList<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;

    private const int DefaultCapacity = 4;
    
    public GaList(int capacity = 4)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");
        
        _items = new T[DefaultCapacity];
        _count = 0;
    }
    
    public int Count => _count; // number of elements in the list

    public T this[int index]
    {
        get => (uint)index < (uint)_count 
            ? _items[index] 
            : throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");
        set
        {
            if ((uint)index >= (uint)_count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");
            
            _items[index] = value;
        }
    }
    
    private void EnsureCapacity(int min)
    {
        if (_items.Length >= min) return; // current capacity is sufficient
        
        var capacity = _items.Length == 0 
            ? DefaultCapacity
            : _items.Length * 2;
        
        if (capacity < min)
            capacity = min;
        
        Array.Resize(ref _items, capacity);
            
    }
    
    // [0, 1, 2, 3] - indexes
    public void Add(T item)
    {
        EnsureCapacity(_count + 1);
        _items[_count++] = item;
    }

    // [0, 1, 2, 3, 4] - indexes
    // [40,10,20,30,0] - contents
    // remove index 2 => [40,10,30,0,0]
    public bool RemoveAt(int index)
    {
        // validate index
        if ((uint)index >= (uint)_count) 
            return false;
        
        // shift elements to the left
        Array.Copy(
            _items,    // source array
            index + 1, // source start index
            _items, // destination array to copy to
            index,               // destination start index
            _count - index); // number of elements to copy
        
        _items[_count] = default!; // clear the last element
        _count--;
        return true;
    }
    
    public void Clear()
    {
        if (_count > 0)
        {
            Array.Clear(
                _items,         // reference to the array
                0,         // starting index
                _count);  // number of elements to clear
            _count = 0;
            _version++;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
             yield return _items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}