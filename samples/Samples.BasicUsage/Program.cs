namespace Arborescence;

using System;
using System.Collections.Generic;
using Models.Specialized;
using Traversal.Specialized;

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
        Int32AdjacencyGraph graph = Int32AdjacencyGraphFactory.FromEdges(edges);

        EnumerableBfs<Int32AdjacencyGraph, Endpoints<int>, IncidenceEnumerator<int, ArraySegment<int>.Enumerator>> bfs;

        using IEnumerator<Endpoints<int>> treeEdges =
            bfs.EnumerateEdges(graph, source: 3, vertexCount: graph.VertexCount);
        while (treeEdges.MoveNext())
            Console.WriteLine(treeEdges.Current);
    }
}
