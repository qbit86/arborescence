namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Models.Specialized;
using Workbench;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<
    int, Endpoints<int>, IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>>;

internal static class Program
{
    private static CultureInfo P => CultureInfo.InvariantCulture;

    private static Endpoints<int> Transform(Int32Endpoints endpoints) => new(endpoints.Tail, endpoints.Head);

    private static void Main()
    {
        using TextReader textReader = IndexedGraphs.GetTextReader("09");
        Endpoints<int>[] edges = IndexedEdgeListParser.ParseEdges(textReader).Select(Transform).ToArray();
        textReader.Dispose();

        Int32AdjacencyGraph graph = Int32AdjacencyGraphFactory.FromEdges(edges);
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

        int[] sources = { 3 };
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet enumerableExploredSet = new(setBackingStore);
        HashSet<Endpoints<int>> treeEdges = new(graph.EdgeCount);
        IEnumerable<Endpoints<int>> steps = EnumerableDfs.EnumerateEdges(graph, sources, enumerableExploredSet);
        foreach (Endpoints<int> e in steps)
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
            IncidenceEnumerator<int, ArraySegment<int>.Enumerator> outEdges = graph.EnumerateOutEdges(v);
            while (outEdges.MoveNext())
            {
                Endpoints<int> e = outEdges.Current;
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
