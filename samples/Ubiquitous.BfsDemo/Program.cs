namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using Models;
    using Traversal;
    using Workbench;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal static class Program
    {
        private static CultureInfo F => CultureInfo.InvariantCulture;

        private static void Main()
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("03"))
            {
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

            var bfs = InstantBfs<AdjacencyListIncidenceGraph, int, int, ArraySegmentEnumerator<int>, byte[]>.Create(
                default(IndexedAdjacencyListGraphPolicy), default(IndexedColorMapPolicy));
            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            TextWriter w = Console.Out;
            BfsHandler<AdjacencyListIncidenceGraph, int, int> handler = CreateHandler(w);

            w.WriteLine("digraph {");
            w.WriteLine("  node [shape=circle fontname=\"Times-Italic\"]");
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                w.Write(v == 0 ? "  " : " ");
                w.Write(V(v));
            }

            w.WriteLine();

            bfs.Traverse(graph, 0, colorMap, handler);

            w.WriteLine("}");

            ArrayPool<byte>.Shared.Return(colorMap);
        }

        private static BfsHandler<AdjacencyListIncidenceGraph, int, int> CreateHandler(TextWriter w)
        {
            Debug.Assert(w != null, "w != null");

            var result = new BfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.DiscoverVertex +=
                (_, v) => w.WriteLine($"  {V(v)} [style=filled] // {nameof(result.DiscoverVertex)}");
            result.ExamineVertex += (_, v) => w.WriteLine($"  // {nameof(result.ExamineVertex)} {V(v)}");
            result.FinishVertex += (_, v) => w.WriteLine($"  // {nameof(result.FinishVertex)} {V(v)}");
            result.ExamineEdge += (g, e) =>
                w.WriteLine($"  // {nameof(result.ExamineEdge)} {e.ToString(F)}: {E(g, e)}");
            return result;
        }

        private static string V(int v) => Base32.ToString(v);

        private static string E(AdjacencyListIncidenceGraph g, int e)
        {
            string head = g.TryGetHead(e, out int h) ? V(h) : "?";
            string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
            return string.Concat(tail, " -> ", head);
        }
    }
}
