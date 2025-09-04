namespace LinearSearch;

public static class Search
{
    // [0,  1,  2, 3,  4, 5] -> indexes
    // [4, 66, 33, 2, 12, 1] -> target = 12 -> returns 4
    // Returns the index of the target if found, otherwise -1
    public static int LinearSearch(int[] array, int target)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
                return i;
        }   
        return -1;
    }
}