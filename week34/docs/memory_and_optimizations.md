# C# Code Optimization Overview

This document provides a structured overview of best practices and techniques for optimizing C# code, focusing on performance and memory efficiency.

## General Optimization Areas

| Area                             | Strategy                                                             | Explanation                                                             |
| -------------------------------- | -------------------------------------------------------------------- | ----------------------------------------------------------------------- |
| **Profiling & Diagnostics**      | Use tools like Visual Studio Profiler, dotTrace, BenchmarkDotNet     | Measure before optimizing. Identify actual bottlenecks.                 |
| **Algorithms & Data Structures** | Use appropriate structures like Dictionary, Span, etc.               | Reduces time and space complexity.                                      |
| **Memory Efficiency**            | Use structs for small data, pool large objects, minimize allocations | Avoid unnecessary GC pressure and heap allocations.                     |
| **Garbage Collection**           | Minimize short-lived allocations, use StringBuilder                  | Reduces GC frequency and improves responsiveness.                       |
| **Object Pooling**               | Use `ArrayPool<T>`, `ObjectPool<T>`, or custom pooling               | Reuse large or expensive objects to reduce allocations and GC pressure. |
| **Code-Level Optimizations**     | Avoid boxing, prefer for-loops, minimize closures                    | Lower allocations and CPU cycles.                                       |
| **String Handling**              | Use StringBuilder, ReadOnlySpan                                      | Avoid large numbers of immutable string operations.                     |
| **Async & Threading**            | Prefer async/await, avoid Task.Run in ASP.NET, use ValueTask         | Prevents thread blocking and improves scalability.                      |
| **I/O Performance**              | Use buffered streams, async file/network calls                       | Reduces blocking and improves throughput.                               |
| **Database Access**              | Optimize queries, batch operations, use connection pooling           | Lowers latency and server load.                                         |
| **Platform Optimizations**       | Use unsafe code only when profiling justifies it                     | Allows low-level tuning, but adds complexity.                           |

<div style="page-break-after:always;"></div>

## Memory Management in C\#

### Memory Segments

| Segment          | Purpose                                                                 | Typical Size & Notes                             |
| ---------------- | ----------------------------------------------------------------------- | ------------------------------------------------ |
| **Stack**        | Stores value types and method call frames. Fast, automatically managed. | Small (typically 1MB per thread), grows downward |
| **Heap**         | Stores reference types and large objects. Managed by the GC.            | Large, slower access, fragmented by GC activity  |
| **Data Segment** | Stores static variables and constants.                                  | Size depends on static/global usage              |

### Value vs Reference Types

| Type Category       | Example Types                        | Memory Location                    |
| ------------------- | ------------------------------------ | ---------------------------------- |
| **Value Types**     | `int`, `double`, `bool`, `struct`    | Stack (unless boxed or in a class) |
| **Reference Types** | `class`, `string`, `object`, `array` | Heap                               |

### Structs and Classes

| Type            | Value/Reference | Default Allocation | Use Case                             |
| --------------- | --------------- | ------------------ | ------------------------------------ |
| `struct`        | Value Type      | Stack              | Small, immutable, short-lived data   |
| `class`         | Reference Type  | Heap               | Larger or shared mutable data        |
| `record struct` | Value Type      | Stack              | Immutable small data, ideal for DTOs |
| `record class`  | Reference Type  | Heap               | Immutable reference objects (C# 9+)  |

### Example

```csharp
struct Point { public int X, Y; }      // Value type
class Car { public string Model; }     // Reference type
record struct Position(int X, int Y);  // Immutable value type
record class User(string Name);        // Immutable reference type
```

**Notes:**

* Value types are copied when assigned.
* Reference types copy references, not the object.
* Value types inside reference types (like fields in a class) live on the heap.
* Stack space is limited; excessive use (e.g., deep recursion or large structs) may cause stack overflow.

<div style="page-break-after:always;"></div>

### Understanding Field and Local Variable Allocation

#### Example 1: Field in a Class

```csharp
class Person {
    public int Age { get; set; }
}
```

* `Person` is a reference type → stored on the **heap**.
* `Age` is a value type → stored **inside the Person object**, on the **heap**.
* ✅ **No boxing occurs**.

#### Example 2: Local Variable in a Method

```csharp
void DoSomething() {
    int localAge = 42;
}
```

* `localAge` is a value type and a local variable → stored on the **stack**.
* Exists only for the duration of the method call.

#### Summary Table

| Location of `int`        | Stored In | Boxed? |
| ------------------------ | --------- | ------ |
| Field in class object    | Heap      | ❌ No   |
| Local variable in method | Stack     | ❌ No   |
| Assigned to `object`     | Heap      | ✅ Yes  |

Boxing only occurs when a value type is converted to a reference type (e.g., `object o = 42;`).

---
<div style="page-break-after:always;"></div>

## Examples, Use Cases & Pitfalls for Each Area

### Profiling & Diagnostics

```csharp
[Benchmark]
public void MyMethod() {
    // Code to measure
}
```

**When to Use:** Always at the start and end of an optimization cycle.
**Pitfalls:** Ignoring metrics can lead to wasted efforts on non-critical code paths.

### Algorithms & Data Structures

```csharp
var dict = new Dictionary<int, string>(); // Fast key lookup
var set = new HashSet<string>(); // Fast membership check
Span<int> nums = stackalloc int[100]; // Avoid heap allocation
```

**When to Use:** When managing large datasets or optimizing for speed/memory.
**Pitfalls:** Using List for frequent lookups or LINQ in tight loops.

### Memory Efficiency

```csharp
struct Point { public int X, Y; } // lightweight
```

**When to Use:** For small, immutable, short-lived data.
**Pitfalls:** Large structs can cause performance drops due to copying.

### Garbage Collection

```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) sb.Append(i);
```

**When to Use:** For repeated string or object creation.
**Pitfalls:** Excessive allocations can cause Gen2 collections and performance dips.

### Object Pooling

```csharp
var buffer = ArrayPool<byte>.Shared.Rent(1024);
// use buffer
ArrayPool<byte>.Shared.Return(buffer);
```

**When to Use:** For buffers, DTOs, or reusable objects in high-frequency use.
**Pitfalls:** Not resetting state or returning objects causes leaks and bugs.

### Code-Level Optimizations

```csharp
for (int i = 0; i < items.Length; i++) Process(items[i]);
```

**When to Use:** In performance-critical loops and utility methods.
**Pitfalls:** Premature optimization, sacrificing readability.

### String Handling

```csharp
ReadOnlySpan<char> span = "Example".AsSpan();
if (span.StartsWith("Ex")) { }
```

**When to Use:** For parsing or substring checks in tight code.
**Pitfalls:** Overusing string concatenation or unnecessary ToString calls.

### Async & Threading

```csharp
public async Task<string> LoadAsync() => await httpClient.GetStringAsync(url);
```

**When to Use:** In I/O-bound or UI/web server code.
**Pitfalls:** Blocking threads with `.Result` or `Task.Run` unnecessarily.

### I/O Performance

```csharp
using var fs = new FileStream("data.bin", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
await fs.ReadAsync(buffer, 0, buffer.Length);
```

**When to Use:** For large file/network operations.
**Pitfalls:** Not buffering or doing sync I/O on async apps.

### Database Access

```csharp
cmd.CommandText = "SELECT name FROM users WHERE id = @id";
cmd.Parameters.AddWithValue("@id", userId);
```

**When to Use:** For reducing load and securing queries.
**Pitfalls:** Using `SELECT *`, building queries with string interpolation.

### Platform Optimizations

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static int Add(int a, int b) => a + b;
```

**When to Use:** In extremely hot paths identified via profiling.
**Pitfalls:** Unnecessary use can increase JIT size and reduce performance.

---

**Note**: Always benchmark before and after optimizations using `BenchmarkDotNet` or Visual Studio's performance profiler.

---

Would you like to add sections with specific code examples or best practices tailored for ASP.NET, games, or desktop apps?
