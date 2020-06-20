namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using Traversal;
    using Workbench;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal static partial class Program
    {
        private static void DemoEnumerableDfs()
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(0, 31);

            using (TextReader textReader = IndexedGraphs.GetTextReader("09"))
            {
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

            TextWriter w = Console.Out;

            w.WriteLine("digraph \"DFS forest\" {");
            w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

            // Enumerate vertices.
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                w.Write(v == 0 ? "  " : " ");
                w.Write(V(v));
            }

            w.WriteLine();

            EnumerableDfs<AdjacencyListIncidenceGraph, int, int, ArraySegmentEnumerator<int>, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy> dfs = default;
            var sources = new IndexEnumerator(2);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(
                IndexedSetPolicy.GetByteCount(graph.VertexCount));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);
            using IEnumerator<int> steps = dfs.EnumerateEdges(graph, sources, enumerableExploredSet);

            ArrayPool<byte>.Shared.Return(enumerableExploredSet);

            w.WriteLine("}");
        }
    }
}
