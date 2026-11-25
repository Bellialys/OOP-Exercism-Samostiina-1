using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public static class Forth
{
    public static string Evaluate(string[] instructions)
    {
        var stack = new Stack<int>();
        var words = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase);
        void Require(int count)
        {
            if (stack.Count < count) throw new InvalidOperationException("Stack underflow.");
        }
        void Binary(Func<int, int, int> operation)
        {
            Require(2);
            int right = stack.Pop(), left = stack.Pop();
            stack.Push(operation(left, right));
        }
        words["+"] = () => Binary((a, b) => a + b);
        words["-"] = () => Binary((a, b) => a - b);
        words["*"] = () => Binary((a, b) => a * b);
        words["/"] = () => Binary((a, b) => a / b);
        words["dup"] = () => { Require(1); stack.Push(stack.Peek()); };
        words["drop"] = () => { Require(1); stack.Pop(); };
        words["swap"] = () =>
        {
            Require(2);
            int top = stack.Pop(), second = stack.Pop();
            stack.Push(top); stack.Push(second);
        };
        words["over"] = () =>
        {
            Require(2);
            int top = stack.Pop(), second = stack.Peek();
            stack.Push(top); stack.Push(second);
        };
        Action Resolve(string token)
        {
            if (IsNumber(token))
            {
                int number = int.Parse(token, CultureInfo.InvariantCulture);
                return () => stack.Push(number);
            }
            if (words.TryGetValue(token, out var action)) return action;
            throw new InvalidOperationException($"Unknown word: {token}");
        }
        string[] tokens = string.Join(" ", instructions).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < tokens.Length; i++)
        {
            string token = tokens[i];
            if (token == ":")
            {
                if (++i >= tokens.Length || IsNumber(tokens[i]) || tokens[i] == ":" || tokens[i] == ";")
                    throw new InvalidOperationException("Invalid word definition.");
                string name = tokens[i];
                var definition = new List<Action>();
                while (++i < tokens.Length && tokens[i] != ";") definition.Add(Resolve(tokens[i]));
                if (i >= tokens.Length) throw new InvalidOperationException("Unterminated definition.");
                Action[] compiled = definition.ToArray();
                words[name] = () => { foreach (Action action in compiled) action(); };
            }
            else Resolve(token)();
        }
        return string.Join(" ", stack.Reverse());
    }
    private static bool IsNumber(string token) => Regex.IsMatch(token, @"^[+-]?[0-9]+$");
}
