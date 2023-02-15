namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Models;
using Traversal;
using Traversal.Incidence;
using Workbench;

internal static class Program
{
    private static CultureInfo F => CultureInfo.InvariantCulture;

    private static void Main()
    {
        IndexedIncidenceGraph.Builder builder = new(0, 31);

        using (TextReader textReader = IndexedGraphs.GetTextReader("09"))
        {
            IEnumerable<Int32Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
            foreach (Int32Endpoints edge in edges)
                builder.Add(edge.Tail, edge.Head);
        }

        IndexedIncidenceGraph graph = builder.ToGraph();
        Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(F)}");
        Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(F)}");

        TextWriter w = Console.Out;

        w.WriteLine($"digraph \"{nameof(EagerDfs<int, int, ArraySegment<int>.Enumerator>)}\" {{");
        w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

        // Enumerate vertices.
        for (int v = 0; v < graph.VertexCount; ++v)
        {
            w.Write(v == 0 ? "  " : " ");
            w.Write(V(v));
        }

        w.WriteLine();

        IndexEnumerator sources = new(2);
        byte[] backingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(backingStore, 0, backingStore.Length);
        IndexedColorDictionary colorByVertex = new(backingStore);
        HashSet<int> examinedEdges = new(graph.EdgeCount);
        DfsHandler<int, int, IndexedIncidenceGraph> handler = CreateHandler(w, examinedEdges);
        EagerDfs<int, int, ArraySegment<int>.Enumerator>.Traverse(graph, sources, colorByVertex, handler);
        ArrayPool<byte>.Shared.Return(backingStore);

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
            ArraySegment<int>.Enumerator outEdges = graph.EnumerateOutEdges(v);
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

    private static DfsHandler<int, int, IndexedIncidenceGraph> CreateHandler(
        TextWriter w, HashSet<int> examinedEdges)
    {
        DfsHandler<int, int, IndexedIncidenceGraph> result = new();
        result.StartVertex += (_, v) => w.WriteLine($"  // {nameof(result.StartVertex)} {V(v)}");
        result.DiscoverVertex += (_, v) => w.WriteLine($"  {V(v)} [style=solid]");
        result.FinishVertex += (_, v) => w.WriteLine($"  // {nameof(result.FinishVertex)} {V(v)}");
        result.ExamineEdge += (_, e) => examinedEdges.Add(e);
        result.TreeEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=bold]");
        result.ForwardOrCrossEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=solid]");
        result.BackEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=dashed]");
        result.FinishEdge += (g, e) => w.WriteLine($"  // {nameof(result.FinishEdge)} {E(g, e)}");
        return result;
    }

    private static string V(int v) => Base32.ToString(v);

    private static string E(IndexedIncidenceGraph g, int e)
    {
        string head = g.TryGetHead(e, out int h) ? V(h) : "?";
        string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
        return string.Concat(tail, " -> ", head);
    }
}