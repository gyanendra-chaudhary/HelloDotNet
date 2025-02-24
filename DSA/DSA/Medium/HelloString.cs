namespace DSA.Medium;

public class HelloString
{
    public int LengthOfLongestSubstring(string s)
    {
        int left = 0;
        int result = 0;
        Dictionary<int, int> charMap = new Dictionary<int, int>();
        for (int right = 0; right < s.Length; right++)
        {
            if (charMap.ContainsKey(s[right]))
            {
                charMap[s[right]]++;
            }
            else 
            {
                charMap.Add(s[right], 1);
            }
            result = Math.Max(result, (right - left));

        }
        return result;
    }
}