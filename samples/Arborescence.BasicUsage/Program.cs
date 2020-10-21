namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Traversal;

    internal static class Program
    {
        private static void Main()
        {
            var builder = new SimpleIncidenceGraph.Builder();
            builder.TryAdd(2, 0);
            builder.TryAdd(3, 2);
            builder.TryAdd(0, 3);
            builder.TryAdd(3, 0);
            SimpleIncidenceGraph graph = builder.ToGraph();

            Bfs<SimpleIncidenceGraph, Endpoints, ArraySegment<Endpoints>.Enumerator> bfs;

            IEnumerator<Endpoints> edges = bfs.EnumerateEdges(graph, graph.VertexCount, 2);
            while (edges.MoveNext())
                Console.WriteLine(edges.Current);
        }
    }
}
