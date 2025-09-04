namespace Recursion;

public static class RecursiveMethods
{
    public static long Factorial(int n)
    {
         if (n < 0) 
             throw new ArgumentException("Negative numbers do not have factorials.");
         
         // Base case -> stopping condition
         if (n <= 1) 
             return 1;
         
         return n * Factorial(n - 1); // Recursive case
    }
}