using System;
using System.Text;
public class StringWordReverse
{
    public string ReverseTheStringWords(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        string[] words = input.Split(" ");
        StringBuilder result = new StringBuilder();
        bool isFistWord = true;
        foreach (var word in words)
        {
            var a = ReverseString(word);
            if (isFistWord)
            {
                isFistWord = false;
                result.Append(a);
            }
            else
            {
                result.Append(" " + a);
            }

        }
        return result.ToString();
    }

    public string ReverseString(string input)
    {
        char[] result = new char[input.Length];
        char[] chars = input.ToCharArray();
        int index = 0;
        for (int i = chars.Length - 1; i >= 0; i--)
        {
            result[index] = chars[i];
            index++;
        }

        return new string(result);
    }
}