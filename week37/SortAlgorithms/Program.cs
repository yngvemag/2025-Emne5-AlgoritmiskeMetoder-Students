

using SortAlgorithms;

int[] array = [12,55,22,4,56,2,33,1,99,23];

Console.WriteLine($"Før sortering: {string.Join(", ", array)}");

// InsertionSort.Sort(array, HandleArray);
InsertionSort.Sort(array, (arr) =>
{
    Console.WriteLine(string.Join(", ", arr));
});

Console.WriteLine($"Etter sortering: {string.Join(", ", array)}");


// Action<int[]>
static void HandleArray(int[] arr) 
{ 
    Console.WriteLine(string.Join(", ", arr)); 
}
