﻿namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Models.Specialized;
using Traversal;
using Traversal.Incidence;
using Workbench;

internal static class Program
{
    private static CultureInfo P => CultureInfo.InvariantCulture;

    private static void Main()
    {
        using var textReader = IndexedGraphs.GetTextReader("09");
        var endpointsByEdge = Base32EdgeListParser.ParseEdges(textReader).ToArray();
        textReader.Close();

        var graph = Int32IncidenceGraph.FromEdges(endpointsByEdge);
        Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(P)}");
        Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(P)}");

        var w = Console.Out;

        w.WriteLine($"digraph \"{nameof(EagerBfs<int, int, ArraySegment<int>.Enumerator>)}\" {{");
        w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

        // Enumerate vertices.
        for (int v = 0; v < graph.VertexCount; ++v)
        {
            w.Write(v == 0 ? "  " : " ");
            w.Write(V(v));
        }

        w.WriteLine();

        int[] sources = { 0, 1 };
        byte[] backingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(backingStore, 0, backingStore.Length);
        Int32ColorDictionary colorByVertex = new(backingStore);
        HashSet<int> examinedEdges = new(graph.EdgeCount);
        var handler = CreateHandler(w, examinedEdges);
        EagerBfs<int, int, ArraySegment<int>.Enumerator>.Traverse(graph, sources, colorByVertex, handler);
        ArrayPool<byte>.Shared.Return(backingStore);

        // Enumerate sources.
        w.WriteLine();
        foreach (int v in sources)
            w.WriteLine($"  {V(v)} [style=filled]");

        // Enumerate rest of edges.
        w.WriteLine();
        for (int v = 0; v < graph.VertexCount; ++v)
        {
            var outEdges = graph.EnumerateOutEdges(v);
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

    private static BfsHandler<int, int, Int32IncidenceGraph> CreateHandler(TextWriter w, HashSet<int> examinedEdges)
    {
        BfsHandler<int, int, Int32IncidenceGraph> result = new();
        result.DiscoverVertex += (_, v) => w.WriteLine($"  {V(v)} [style=solid]");
        result.ExamineVertex += (_, v) => w.WriteLine($"  // {nameof(result.ExamineVertex)} {V(v)}");
        result.FinishVertex += (_, v) => w.WriteLine($"  // {nameof(result.FinishVertex)} {V(v)}");
        result.ExamineEdge += (_, e) => examinedEdges.Add(e);
        result.TreeEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=bold]");
        result.NonTreeGrayHeadEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e}]");
        result.NonTreeBlackHeadEdge += (g, e) => w.WriteLine($"  {E(g, e)} [label={e} style=dashed]");
        return result;
    }

    private static string V(int v) => Base32.ToString(v);

    private static string E(Int32IncidenceGraph g, int e)
    {
        string head = g.TryGetHead(e, out int h) ? V(h) : "?";
        string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
        return string.Concat(tail, " -> ", head);
    }
}
