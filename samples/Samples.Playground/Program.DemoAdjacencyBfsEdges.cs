namespace Arborescence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Specialized;
using Traversal.Adjacency;
using Workbench;

internal static partial class Program
{
    private static void DemoAdjacencyBfsEdges()
    {
        using TextReader textReader = IndexedGraphs.GetTextReader("09");
        var edges = Base32EdgeListParser.ParseEdges(textReader).ToList();
        textReader.Dispose();

        Int32AdjacencyGraph adjacencyGraph = Int32AdjacencyGraphFactory.FromEdges(edges);
        IEnumerable<int> sources = "abcd".Select(it => Base32.Parse(it.ToString()));
        IEnumerable<Endpoints<int>> arrows = EnumerableBfs<int, ArraySegment<int>.Enumerator>.EnumerateEdges(
            adjacencyGraph, sources);

        foreach (Endpoints<int> arrow in arrows)
        {
            (int tail, int head) = arrow;
            Console.WriteLine($"{V(tail)} -> {V(head)}");
        }
    }
}
