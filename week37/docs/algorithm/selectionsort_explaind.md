# 🧠 How Selection Sort Works — Step by Step

## Step 1: Start at the Beginning

- Look through the **entire list** to find the **smallest element**.  
- Swap it with the **first element**.

## Step 2: Move to the Next Position

- Ignore the first element (already in place).  
- Find the next smallest element in the rest of the list.  
- Swap it with the **second element**.

## Step 3: Keep Going

- Continue finding the smallest element in the remaining unsorted portion and move it to the front.  
- Repeat until all elements are sorted.

---

## 🔁 Example

```
[29, 10, 14, 37, 13]
```

### Pass 1

- Smallest is 10 → swap with 29  
- `[10, 29, 14, 37, 13]`

### Pass 2

- Smallest in `[29, 14, 37, 13]` is 13  
- Swap with 29  
- `[10, 13, 14, 37, 29]`

### Pass 3

- Smallest in `[14, 37, 29]` is 14 → already in place  
- `[10, 13, 14, 37, 29]`

### Pass 4

- Smallest in `[37, 29]` is 29 → swap with 37  
- `[10, 13, 14, 29, 37]`

✅ Sorted.

---

## ✅ Summary

- Find the **smallest element** in the unsorted part  
- Swap it with the first unsorted position  
- Repeat until sorted  

Selection Sort is simple but also **inefficient for large lists** (`O(n²)`).