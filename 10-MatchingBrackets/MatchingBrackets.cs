using System.Collections.Generic;

public static class MatchingBrackets
{
    public static bool IsPaired(string input)
    {
        var stack = new Stack<char>();
        foreach (char symbol in input)
        {
            switch (symbol)
            {
                case '(':
                case '[':
                case '{': stack.Push(symbol); break;
                case ')':
                case ']':
                case '}':
                    if (stack.Count == 0 || !IsMatchingPair(stack.Pop(), symbol)) return false;
                    break;
            }
        }
        return stack.Count == 0;
    }
    private static bool IsMatchingPair(char opening, char closing) => (opening, closing) switch
    {
        ('(', ')') => true, ('[', ']') => true, ('{', '}') => true, _ => false
    };
}
