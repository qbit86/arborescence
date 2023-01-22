namespace Arborescence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Traversal.Adjacency;
using Workbench;
using EdgeEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;

internal static partial class Program
{
    private static void DemoAdjacencyBfsEdges()
    {
        SimpleIncidenceGraph.Builder builder = new(18);
        using (TextReader textReader = IndexedGraphs.GetTextReader("09"))
        {
            IEnumerable<Int32Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
            foreach (Int32Endpoints edge in edges)
                builder.Add(edge.Tail, edge.Head);
        }

        SimpleIncidenceGraph incidenceGraph = builder.ToGraph();
        var adjacencyGraph = IncidenceAdjacencyAdapter<int, Int32Endpoints, EdgeEnumerator>.Create(incidenceGraph);
        IEnumerable<int> sources = "abcd".Select(it => Base32.Parse(it.ToString()));
        IEnumerable<Endpoints<int>> arrows =
            EnumerableBfs<int>.EnumerateEdges(adjacencyGraph, sources.GetEnumerator());
        foreach (Endpoints<int> arrow in arrows)
        {
            (int tail, int head) = arrow;
            Console.WriteLine($"{V(tail)} -> {V(head)}");
        }
    }
}
