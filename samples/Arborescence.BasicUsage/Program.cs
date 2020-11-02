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
            builder.Add(2, 0);
            builder.Add(4, 3);
            builder.Add(0, 4);
            builder.Add(3, 2);
            builder.Add(4, 4);
            builder.Add(0, 2);
            builder.Add(2, 4);
            SimpleIncidenceGraph graph = builder.ToGraph();

            Bfs<SimpleIncidenceGraph, Endpoints, ArraySegment<Endpoints>.Enumerator> bfs;

            IEnumerator<Endpoints> edges = bfs.EnumerateEdges(graph, source: 3, vertexCount: graph.VertexCount);
            while (edges.MoveNext())
                Console.WriteLine(edges.Current);
        }
    }
}
