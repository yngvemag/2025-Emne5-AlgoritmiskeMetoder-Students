namespace SortAlgorithms;


public static class InsertionSort
{
    public static void Sort(
        int[] array,
        Action<int[]> handleArray = null)
    {
        // Ytre løkke som går gjennom hvert element i arrayet, starter fra det andre elementet
        for (int i = 1; i < array.Length; i++)
        {
            // Lagre det nåværende elementet som skal settes inn
            int key = array[i];

            // Indeks for det forrige elementet
            int j = i - 1;
            while(j >= 0 && array[j] > key)
            {
                // Flytt elementet ett sted til høyre for å lage plass for key
                array[j + 1] = array[j];
                j--; // Flytt til forrige element
            }
            // Sett inn key på riktig plass
            array[j + 1] = key; // j+1 fordi j har blitt redusert en ekstra gang i while-løkken
            
            if (handleArray != null)
            {
                handleArray(array);
            }
            
        }
    }    
}