namespace DSA.Recursion;

public class Beginner
{
    public int FactorialCalculation(int n)
    {
        // checking if n is negative
        if (n < 0)
            throw new ArgumentException(nameof(n), "n must be positive");
        int result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }

        return result;
    }

    public int FactorialRecursive(int n)
    {
        if(n < 0)
            throw new ArgumentException(nameof(n), "n must be positive");
        if (n <= 1)
        {
            return 1;
        }
        return n*FactorialRecursive(n - 1);
    }
}