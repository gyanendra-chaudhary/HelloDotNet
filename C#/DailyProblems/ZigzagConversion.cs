using System;
using System.Text;
public class ZigzagConversion
{
     public string Convert(string s, int numRows) {
        if(numRows ==0 ) return s;
        if(s.Length==0) return null;
        int gap = numRows>=2?numRows - 2:0;
        int escapeIndex = numRows + gap;
        
        StringBuilder result = new StringBuilder();
        for(int i =0; i<numRows; i++)
        {
            int rowIteration = i;
            while(rowIteration < s.Length){
                result.Append(s[rowIteration]);
                rowIteration += escapeIndex;
            }
        }
        return result.ToString();
    }
}