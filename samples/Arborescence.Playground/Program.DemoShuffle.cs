namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using Workbench;

    internal static partial class Program
    {
        // ReSharper disable once UnusedMember.Local
        private static void DemoShuffle()
        {
            var builder = new IndexedIncidenceGraph.Builder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("08"))
            {
                IEnumerable<Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head);
            }

            IndexedIncidenceGraph graph = builder.ToGraph();
            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

            TextWriter w = Console.Out;

            w.WriteLine("digraph Shuffled {");
            w.WriteLine("  node [shape=circle fontname=\"Times-Italic\"]");

            // Enumerate vertices.
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                w.Write(v == 0 ? "  " : " ");
                w.Write(V(v));
            }

            w.WriteLine();

            int[] permutation = Enumerable.Range(0, graph.EdgeCount)
                .OrderBy(i => new Random(i).Next()).ToArray();

            for (int i = 0; i < graph.EdgeCount; ++i)
            {
                int e = permutation[i];
                w.Write("  ");
                w.Write(E(graph, e));
                w.WriteLine($" [label={i.ToString(F)}]");
            }

            w.WriteLine("}");
        }
    }
}
