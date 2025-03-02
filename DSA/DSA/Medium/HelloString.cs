namespace DSA.Medium
{
    public class HelloString
    {
        public int LengthOfLongestSubstring(string s)
        {
            int left = 0;
            int result = 0;
            Dictionary<char, int> charMap = new Dictionary<char, int>();
            for (int right = 0; right < s.Length; right++)
            {
                if (charMap.ContainsKey(s[right]))
                {
                    left = Math.Max(charMap[s[right]] + 1, left);
                }
                charMap[s[right]] = right;
                result = Math.Max(result, right - left + 1);
            }
            return result;
        }
        public string ZigZagConversion(string s, int numRows)
        {
            int row = numRows;
            int col = (int)(Math.Ceiling((double)s.Length / (numRows * 2 - 2)));
            int [,] chartbl = new int[row, col];
            return "";
        }
    }
}