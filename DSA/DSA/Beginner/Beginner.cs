
using System.Numerics;
using System.Text;
using System.Xml;

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
        if (n < 0)
            throw new ArgumentException(nameof(n), "n must be positive");
        if (n <= 1)
        {
            return 1;
        }
        return n * FactorialRecursive(n - 1);
    }

    public ListNode MergeTwoLists(ListNode list1, ListNode list2)
    {
        return new ListNode();
    }

    // remove duplicates
    public int RemoveDuplicates(int[] nums)
    {
        if (nums.Length == 0)
            return 0;

        int pointer = 0;
        for (int i = 1; i < nums.Length; i++)
        {
            if (nums[pointer] != nums[i])
            {
                nums[++pointer] = nums[i];
            }

        }
        return pointer + 1;
    }

    public int SearchInsert(int[] nums, int target)
    {

        int result = Array.IndexOf(nums, target);
        if (result < 0)
        {
            var a = Array.Find(nums, x => x > target);
            if (a == 0)
            {
                return nums.Length;
            }
            result = Array.IndexOf(nums, a);
            return result;
        }
        return result;
    }
    public int LengthOfLastWord(string s)
    {
        s = s.Trim();
        var result = s.Split(' ');
        return result[result.Length - 1].Length;
    }

    public int[] PlusOne(int[] digits)
    {
        for (int i = digits.Length - 1; i <= 0; i++)
        {
            digits[i] += 1;
            if (digits[i] != 10)
            {
                return digits;
            }
            digits[i] = 0;
        }
        int[] result = new int[digits.Length + 1];
        result[0] = 1;
        return result;

    }

    public string AddBinary(string a, string b)
    {
        StringBuilder result = new StringBuilder();
        int carry = 0, i = a.Length - 1, j = b.Length - 1;

        while (i >= 0 || j >= 0 || carry > 0)
        {
            int sum = carry;

            if (i >= 0) sum += a[i--] - '0';
            if (j >= 0) sum += b[j--] - '0';

            result.Append(sum % 2); // Append remainder (0 or 1)
            carry = sum / 2;        // Carry will be 1 if sum is 2 or 3
        }

        return new string(result.ToString().Reverse().ToArray()); // Reverse since we built it backwards
    }

}