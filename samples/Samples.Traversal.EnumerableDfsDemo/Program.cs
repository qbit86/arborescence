﻿namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Models.Specialized;
using Workbench;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<
    int, Endpoints<int>, Models.IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>>;

internal static class Program
{
    private static CultureInfo P => CultureInfo.InvariantCulture;

    private static void Main()
    {
        using var textReader = IndexedGraphs.GetTextReader("09");
        var edges = Base32EdgeListParser.ParseEdges(textReader).ToArray();
        textReader.Close();

        var graph = Int32AdjacencyGraph.FromEdges(edges);
        Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(P)}");
        Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(P)}");

        var w = Console.Out;

        w.WriteLine($"digraph \"{nameof(EnumerableDfs)}\" {{");
        w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

        // Enumerate vertices.
        for (int v = 0; v < graph.VertexCount; ++v)
        {
            w.Write(v == 0 ? "  " : " ");
            w.Write(V(v));
        }

        w.WriteLine();

        int[] sources = { 3 };
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        Int32Set enumerableExploredSet = new(setBackingStore);
        HashSet<Endpoints<int>> treeEdges = new(graph.EdgeCount);
        var steps = EnumerableDfs.EnumerateEdges(graph, sources, enumerableExploredSet);
        foreach (var e in steps)
        {
            w.WriteLine($"  {E(graph, e)} [style=bold]");
            treeEdges.Add(e);

            if (graph.TryGetHead(e, out int v))
                w.WriteLine($"  {V(v)} [style=solid]");
        }

        ArrayPool<byte>.Shared.Return(setBackingStore);

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
                var e = outEdges.Current;
                if (treeEdges.Contains(e))
                    continue;

                w.WriteLine($"  {E(graph, e)} [style=dotted]");
            }
        }

        w.WriteLine("}");
    }

    private static string V(int v) => Base32.ToString(v);

    private static string E(Int32AdjacencyGraph g, Endpoints<int> e)
    {
        string head = g.TryGetHead(e, out int h) ? V(h) : "?";
        string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
        return string.Concat(tail, " -> ", head);
    }
}
