namespace Arborescence;

using System;
using Models.Specialized;
using Traversal.Adjacency;

internal static class Program
{
    private static void Main()
    {
        Endpoints<int>[] edges =
        {
            new(2, 0),
            new(4, 3),
            new(0, 4),
            new(3, 2),
            new(4, 4),
            new(0, 2),
            new(2, 4)
        };
        var graph = Int32AdjacencyGraph.FromEdges(edges);

        var treeEdges = EnumerableBfs<int, ArraySegment<int>.Enumerator>
            .EnumerateEdges(graph, 3);
        foreach (var edge in treeEdges)
            Console.WriteLine(edge);
    }
}
