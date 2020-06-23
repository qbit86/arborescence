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

            EnumerableDfs<AdjacencyListIncidenceGraph, int, int, ArraySegmentEnumerator<int>, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy> dfs = default;

            w.WriteLine($"digraph \"{dfs.GetType().Name}\" {{");
            w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

            // Enumerate vertices.
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                w.Write(v == 0 ? "  " : " ");
                w.Write(V(v));
            }

            w.WriteLine();

            var sources = new IndexEnumerator(1);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(
                IndexedSetPolicy.GetByteCount(graph.VertexCount));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);
            var treeEdges = new HashSet<int>(graph.EdgeCount);
            using IEnumerator<int> steps = dfs.EnumerateEdges(graph, sources, enumerableExploredSet);
            while (steps.MoveNext())
            {
                int e = steps.Current;
                w.WriteLine($"  {E(graph, e)} [label={e} style=bold]");
                treeEdges.Add(e);

                if (graph.TryGetHead(e, out int v))
                    w.WriteLine($"  {V(v)} [style=solid]");
            }

            ArrayPool<byte>.Shared.Return(enumerableExploredSet);

            // Enumerate sources.
            w.WriteLine();
            sources.Reset();
            while (sources.MoveNext())
            {
                int v = sources.Current;
                w.WriteLine($"  {V(v)} [style=filled]");
            }

            // Enumerate rest of edges.
            w.WriteLine();
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                ArraySegmentEnumerator<int> outEdges = graph.EnumerateOutEdges(v);
                while (outEdges.MoveNext())
                {
                    int e = outEdges.Current;
                    if (treeEdges.Contains(e))
                        continue;

                    w.WriteLine($"  {E(graph, e)} [label={e} style=dotted]");
                }
            }

            w.WriteLine("}");
        }
    }
}
