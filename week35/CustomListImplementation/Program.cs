
using CustomListImplementation;

// [0,1,2,3] 
// [0,0,0,0] - contents
GaList<int> gaList = new();
gaList.Add(10); // _count = 1, [10,0,0,0]
gaList.Add(20); // _count = 2, [10,20,0,0]
gaList.Add(30); // _count = 3, [10,20,30,0]
gaList.Add(40); // _count = 4, [10,20,30,40]
    
gaList.Add(50); // _count = 5, capacity increased, [10,20,30,40,50,0,0,0]

//public T this[index]
gaList[1] = 15; // [10,15,30,0]

// iterere over galist
foreach (var item in gaList)
{
    Console.WriteLine(item);
}
