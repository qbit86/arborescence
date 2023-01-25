namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Models;
using Workbench;
using EnumerableDfs =
    Traversal.Incidence.EnumerableDfs<int, Int32Endpoints, System.ArraySegment<Int32Endpoints>.Enumerator>;

internal static class Program
{
    private static CultureInfo P => CultureInfo.InvariantCulture;

    private static void Main()
    {
        SimpleIncidenceGraph.Builder builder = new(0, 31);

        using (TextReader textReader = IndexedGraphs.GetTextReader("09"))
        {
            IEnumerable<Int32Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
            foreach (Int32Endpoints edge in edges)
                builder.Add(edge.Tail, edge.Head);
        }

        SimpleIncidenceGraph graph = builder.ToGraph();
        Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount.ToString(P)}");
        Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount.ToString(P)}");

        TextWriter w = Console.Out;

        w.WriteLine($"digraph \"{nameof(EnumerableDfs)}\" {{");
        w.WriteLine("  node [shape=circle style=dashed fontname=\"Times-Italic\"]");

        // Enumerate vertices.
        for (int v = 0; v < graph.VertexCount; ++v)
        {
            w.Write(v == 0 ? "  " : " ");
            w.Write(V(v));
        }

        w.WriteLine();

        static IEnumerator<int> EnumerateSources() { yield return 3; }
        IEnumerator<int> sources = EnumerateSources();
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet enumerableExploredSet = new(setBackingStore);
        HashSet<Int32Endpoints> treeEdges = new(graph.EdgeCount);
        IEnumerable<Int32Endpoints> steps = EnumerableDfs.EnumerateEdges(graph, sources, enumerableExploredSet);
        foreach (Int32Endpoints e in steps)
        {
            w.WriteLine($"  {E(graph, e)} [style=bold]");
            treeEdges.Add(e);

            if (graph.TryGetHead(e, out int v))
                w.WriteLine($"  {V(v)} [style=solid]");
        }

        ArrayPool<byte>.Shared.Return(setBackingStore);

        // Enumerate sources.
        w.WriteLine();
        sources = EnumerateSources();
        while (sources.MoveNext())
        {
            int v = sources.Current;
            w.WriteLine($"  {V(v)} [style=filled]");
        }

        // Enumerate rest of edges.
        w.WriteLine();
        for (int v = 0; v < graph.VertexCount; ++v)
        {
            ArraySegment<Int32Endpoints>.Enumerator outEdges = graph.EnumerateOutEdges(v);
            while (outEdges.MoveNext())
            {
                Int32Endpoints e = outEdges.Current;
                if (treeEdges.Contains(e))
                    continue;

                w.WriteLine($"  {E(graph, e)} [style=dotted]");
            }
        }

        w.WriteLine("}");
    }

    private static string V(int v) => Base32.ToString(v);

    private static string E(SimpleIncidenceGraph g, Int32Endpoints e)
    {
        string head = g.TryGetHead(e, out int h) ? V(h) : "?";
        string tail = g.TryGetTail(e, out int t) ? V(t) : "?";
        return string.Concat(tail, " -> ", head);
    }
}
