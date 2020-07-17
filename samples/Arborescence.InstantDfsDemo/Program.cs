namespace Arborescence
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

    internal static class Program
    {
        private static CultureInfo F => CultureInfo.InvariantCulture;

        private static void Main()
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

            InstantDfs<AdjacencyListIncidenceGraph, int, int, ArraySegmentEnumerator<int>, byte[],
                IndexedIncidenceGraphPolicy, IndexedColorMapPolicy> dfs = default;

            w.WriteLine($"digraph \"{dfs.GetType().Name}\" {{");
            w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

            // Enumerate vertices.
            for (int v = 0; v < graph.VertexCount; ++v)
            {
                w.Write(v == 0 ? "  " : " ");
                w.Write(V(v));
            }

            w.WriteLine();

            var sources = new IndexEnumerator(2);
            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var examinedEdges = new HashSet<int>(graph.EdgeCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> handler = CreateHandler(w, examinedEdges);
            dfs.Traverse(graph, sources, colorMap, handler);
            ArrayPool<byte>.Shared.Return(colorMap);

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
                    if (examinedEdges.Contains(e))
                        continue;

                    w.WriteLine($"  {E(graph, e)} [label={e} style=dotted]");
                }
            }

            w.WriteLine("}");
        }

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateHandler(
            TextWriter w, HashSet<int> examinedEdges)
        {
            Debug.Assert(w != null, "w != null");
            Debug.Assert(examinedEdges != null, "examinedEdges != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.StartVertex += (_, v) => w.WriteLine($"  // {nameof(result.StartVertex)} {V(v)}");
            result.DiscoverVertex += (_, v) => w.WriteLine($"  {V(v)} [style=solid]");
            result.FinishVertex += (_, v) => w.WriteLine($"  // {nameof(result.FinishVertex)} {V(v)}");
            result.ExamineEdge += (g, e) => examinedEdges.Add(e);
            result.TreeEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=bold]");
            result.ForwardOrCrossEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=solid]");
            result.BackEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=dashed]");
            result.FinishEdge += (g, e) => w.WriteLine($"  // {nameof(result.FinishEdge)} {E(g, e)}");
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
