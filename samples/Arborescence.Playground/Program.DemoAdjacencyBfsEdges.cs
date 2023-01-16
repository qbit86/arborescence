namespace Arborescence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Traversal.Adjacency;
using Workbench;
using EdgeEnumerator = System.ArraySegment<Endpoints>.Enumerator;

internal static partial class Program
{
    private static void DemoAdjacencyBfsEdges()
    {
        SimpleIncidenceGraph.Builder builder = new(18);
        using (TextReader textReader = IndexedGraphs.GetTextReader("09"))
        {
            IEnumerable<Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
            foreach (Endpoints edge in edges)
                builder.Add(edge.Tail, edge.Head);
        }

        SimpleIncidenceGraph incidenceGraph = builder.ToGraph();
        var adjacencyGraph = IncidenceAdjacencyAdapter<int, Endpoints, EdgeEnumerator>.Create(incidenceGraph);
        IEnumerable<int> sources = "abcd".Select(it => Base32.Parse(it.ToString()));
        IEnumerator<(int Tail, int Head)> edgeEnumerator =
            EnumerableBfs<int>.EnumerateEdges(adjacencyGraph, sources.GetEnumerator());
        while (edgeEnumerator.MoveNext())
        {
            (int tail, int head) = edgeEnumerator.Current;
            Console.WriteLine($"{V(tail)} -> {V(head)}");
        }
    }
}
