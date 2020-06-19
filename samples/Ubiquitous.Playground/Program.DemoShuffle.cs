namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Workbench;

    internal static partial class Program
    {
        private static void DemoShuffle()
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("08"))
            {
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

            TextWriter w = Console.Out;

            w.WriteLine("digraph Ordered {");
            w.WriteLine("  node [shape=circle fontname=\"Times-Italic\"]");

            // Enumerate vertices.
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                w.Write(v == 0 ? "  " : " ");
                w.Write(V(v));
            }

            w.WriteLine();

            for (int i = 0; i < graph.EdgeCount; ++i)
            {
                int e = i;
                w.Write("  ");
                w.Write(E(graph, e));
                w.WriteLine($" [label={e.ToString(F)}]");
            }

            w.WriteLine("}");
        }
    }
}
