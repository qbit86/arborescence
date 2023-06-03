namespace Arborescence;

using System;
using System.Collections.Generic;
using Models.Specialized;
using Traversal.Adjacency;

internal static partial class Program
{
    private static void DemoAdjacencyBfsEdges()
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
        Int32AdjacencyGraph graph =
            Int32AdjacencyGraphFactory.FromEdges(edges);
        IEnumerable<Endpoints<int>> treeEdges =
            EnumerableBfs<int, ArraySegment<int>.Enumerator>.EnumerateEdges(
                graph, source: 3);
        foreach (Endpoints<int> edge in treeEdges)
            Console.WriteLine(edge);
    }
}
