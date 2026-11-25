using System.Collections.Generic;
using System.Linq;

public static class Dominoes
{
    public static bool CanChain(IEnumerable<(int, int)> dominoes)
    {
        var graph = new Dictionary<int, List<int>>();
        foreach (var (left, right) in dominoes)
        {
            if (!graph.ContainsKey(left)) graph[left] = new List<int>();
            if (!graph.ContainsKey(right)) graph[right] = new List<int>();
            graph[left].Add(right);
            graph[right].Add(left);
        }
        if (graph.Count == 0) return true;
        if (graph.Values.Any(neighbors => neighbors.Count % 2 != 0)) return false;
        var visited = new HashSet<int>();
        var stack = new Stack<int>();
        stack.Push(graph.Keys.First());
        while (stack.Count > 0)
        {
            int current = stack.Pop();
            if (!visited.Add(current)) continue;
            foreach (int neighbor in graph[current])
                if (!visited.Contains(neighbor)) stack.Push(neighbor);
        }
        return visited.Count == graph.Count;
    }
}
