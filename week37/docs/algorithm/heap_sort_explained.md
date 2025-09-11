# 🌲 Heap Sort Explained in Plain English

Heap Sort is a **comparison-based sorting algorithm** that uses a special tree structure called a **heap**.  
It sorts by first building a **max-heap** (where the largest element is at the root), and then repeatedly moving the root to the end of the list and shrinking the heap.

---

## 🧠 How Heap Sort Works — Step by Step

### Step 1: Build a Max-Heap
- Arrange the array so that it satisfies the **heap property**:  
  Each parent node is **greater than or equal to** its children.

### Step 2: Swap Root with Last Element
- The **largest element** (root of the heap) is swapped with the last element in the array.  
- This places the largest element in its correct position at the end.

### Step 3: Heapify the Reduced Heap
- Reduce the heap size by one (ignore the sorted part at the end).  
- Restore the heap property (re-heapify).

### Step 4: Repeat
- Continue swapping the root with the last unsorted element and re-heapifying until the array is sorted.

---

## 🔁 Example

Let’s sort this list:

```
[4, 10, 3, 5, 1]
```

### Step 1: Build Max-Heap
```
       10
     /    \
    5      3
   / \
  4   1

Array: [10, 5, 3, 4, 1]
```

### Step 2: Swap Root with Last
- Swap 10 and 1 → `[1, 5, 3, 4, 10]`
- Re-heapify → `[5, 4, 3, 1, 10]`

### Step 3: Swap Root with Last (again)
- Swap 5 and 1 → `[1, 4, 3, 5, 10]`
- Re-heapify → `[4, 1, 3, 5, 10]`

### Step 4: Continue
- Swap 4 and 3 → `[3, 1, 4, 5, 10]`
- Re-heapify → `[3, 1, 4, 5, 10]`
- Swap 3 and 1 → `[1, 3, 4, 5, 10]`

✅ Sorted array: `[1, 3, 4, 5, 10]`

---

## ✅ Summary
- Build a **max-heap**  
- Swap the root (largest) with the last element  
- Reduce heap size and re-heapify  
- Repeat until sorted  

---

## ⏱️ Time and Space Complexity
- **Best case:** `O(n log n)`  
- **Average case:** `O(n log n)`  
- **Worst case:** `O(n log n)`  
- **Space complexity:** `O(1)` (in-place)  
- **Stability:** Not stable (equal elements may change order).

---

## ⚖️ Pros and Cons

### Pros
- Guaranteed `O(n log n)` runtime.
- Works in-place (no extra memory needed).
- Not affected by input order.

### Cons
- Not stable.
- Slower in practice than Quick Sort (because of more data movement).

---

## 📌 Use Cases
- Good when you need guaranteed `O(n log n)` time without extra memory.  
- Often used in **priority queues** and scheduling systems (heap structure itself).

---

## 🛞 Pseudocode

```text
PROCEDURE HEAPSORT(A)
    n ← LENGTH(A)
    BUILD_MAX_HEAP(A)

    heapSize ← n
    FOR end ← n-1 DOWNTO 1 DO
        SWAP(A[0], A[end])        // flytt største til slutten
        heapSize ← heapSize - 1   // krymp heapen
        SIFT_DOWN(A, 0, heapSize) // heapify roten i gjenværende heap
    END FOR
END PROCEDURE


PROCEDURE BUILD_MAX_HEAP(A)
    n ← LENGTH(A)
    // start på siste indre node og gå oppover
    FOR i ← ⌊n/2⌋ - 1 DOWNTO 0 DO
        SIFT_DOWN(A, i, n)
    END FOR
END PROCEDURE


// Heapify / sift-down ved indeks i, gitt heap-størrelse 'heapSize'
PROCEDURE SIFT_DOWN(A, i, heapSize)
    WHILE TRUE DO
        left  ← 2*i + 1
        right ← 2*i + 2
        largest ← i

        IF left  < heapSize AND A[left]  > A[largest] THEN largest ← left  END IF
        IF right < heapSize AND A[right] > A[largest] THEN largest ← right END IF

        IF largest = i THEN
            BREAK                    // heap-egenskap gjenopprettet
        END IF

        SWAP(A[i], A[largest])
        i ← largest                  // fortsett nedover
    END WHILE
END PROCEDURE


```
