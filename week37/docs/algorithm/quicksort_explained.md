# 🧠 Quicksort Explained in Plain English

Quicksort is a **divide and conquer** algorithm. That means it works by breaking a problem down into smaller parts, solving those, and then combining the results.

---

## 🔄 How Quicksort Works — Step by Step

### Step 1: Choose a Pivot
- Pick **one element** in the list to be the **pivot**.
- A common choice is the **last element** in the list.

---

### Step 2: Partition the List
- Go through all the other elements in the list.
- Put:
  - All **smaller** elements to the **left** of the pivot
  - All **larger** elements to the **right** of the pivot

> At this point, the pivot is in its **correct final position** in the sorted list.

---

### Step 3: Recursively Sort the Two Halves
- Now apply the same process (choose pivot, partition, etc.) to:
  - the left part (elements smaller than pivot)
  - the right part (elements larger than pivot)

This continues until each part has 0 or 1 elements, which means they’re already sorted.

---
<div style="page-break-after: always;"></div>

## 🔁 Example

Let’s sort this list:

```
[5, 3, 7, 2, 4]
```

1. **Pick a pivot** → Let's say `4`

2. **Partition**:
   - Left: `[3, 2]` (smaller than 4)
   - Pivot: `4`
   - Right: `[5, 7]` (larger than 4)

List becomes:
```
[3, 2] + [4] + [5, 7]
```

3. **Recurse on `[3, 2]`**:
   - Pick `2` as pivot → Left: `[]`, Pivot: `2`, Right: `[3]`
   - Sorted: `[2, 3]`

4. **Recurse on `[5, 7]`**:
   - Pick `7` as pivot → Left: `[5]`, Pivot: `7`, Right: `[]`
   - Sorted: `[5, 7]`

5. **Combine all**:
```
[2, 3] + [4] + [5, 7] = [2, 3, 4, 5, 7]
```

---

## ✅ Summary

- Choose a pivot
- Partition the list into smaller and larger elements
- Recursively quicksort the two halves
- Combine everything

---

Quicksort is efficient and elegant. It typically performs better than simpler algorithms like bubble sort or insertion sort, especially for large lists.
