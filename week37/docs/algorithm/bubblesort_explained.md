# 🔁 Bubble Sort Explained in Plain English

Bubble Sort is one of the simplest sorting algorithms to understand and implement. It works by **repeatedly stepping through the list**, comparing each pair of adjacent elements, and **swapping them if they are in the wrong order**.

---

## 🧠 How Bubble Sort Works — Step by Step

### Step 1: Start at the Beginning

- Look at the **first two elements** in the list.
- If the **left** element is **greater than** the right one, **swap** them.

---

### Step 2: Move to the Next Pair

- Continue comparing and possibly swapping the **next two elements**, and so on.
- This continues **all the way to the end** of the list.

> At the end of this pass, the **largest element is "bubbled" to the end**.

---

### Step 3: Repeat the Process

- Go back to the beginning and repeat the process.
- Each time, you go one step less far (since the largest elements are already sorted at the end).

Repeat this until **no swaps are needed**, which means the list is sorted.

---
<div style="page-break-after: always;"></div>

## 🔁 Example

Let’s sort this list:

```
[5, 1, 4, 2, 8]
```

### Pass 1

- Compare 5 and 1 → swap → `[1, 5, 4, 2, 8]`
- Compare 5 and 4 → swap → `[1, 4, 5, 2, 8]`
- Compare 5 and 2 → swap → `[1, 4, 2, 5, 8]`
- Compare 5 and 8 → no swap

Result after Pass 1: `[1, 4, 2, 5, 8]` (8 is in correct position)

### Pass 2

- Compare 1 and 4 → no swap
- Compare 4 and 2 → swap → `[1, 2, 4, 5, 8]`
- Compare 4 and 5 → no swap

Result after Pass 2: `[1, 2, 4, 5, 8]`

### Pass 3

- Compare 1 and 2 → no swap
- Compare 2 and 4 → no swap

✅ No swaps this round → Sorting is done.

---

## ✅ Summary

- Compare each pair of adjacent elements
- Swap if the left is bigger than the right
- After each pass, the biggest unsorted element is in place
- Repeat until no swaps are needed

---

Bubble Sort is **easy to understand**, but **not very efficient** for large lists. It's mostly used for **educational purposes**.
