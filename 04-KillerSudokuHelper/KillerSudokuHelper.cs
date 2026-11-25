using System.Collections.Generic;

public static class KillerSudokuHelper
{
    public static IEnumerable<int[]> Combinations(int sum, int size, int[] exclude)
    {
        var results = new List<int[]>();
        var current = new List<int>();
        var excluded = new HashSet<int>(exclude);
        void Search(int start, int remainingSum, int remainingSize)
        {
            if (remainingSize == 0)
            {
                if (remainingSum == 0) results.Add(current.ToArray());
                return;
            }
            for (int digit = start; digit <= 9; digit++)
            {
                if (excluded.Contains(digit)) continue;
                if (digit > remainingSum) break;
                current.Add(digit);
                Search(digit + 1, remainingSum - digit, remainingSize - 1);
                current.RemoveAt(current.Count - 1);
            }
        }
        Search(1, sum, size);
        return results;
    }
}
