namespace Arborescence;

using System;
using System.Collections.Generic;
using Models;

internal static class Program
{
    internal static void Main()
    {
        DemoAdjacencyListGraph();
        DemoIncidenceListGraph();
    }

    private static void DemoAdjacencyListGraph()
    {
        Dictionary<string, List<string>> neighborsByVertex = new(4)
        {
            ["OMS"] = new(),
            ["LHR"] = new() { "IST", "IST" },
            ["IST"] = new() { "LHR" /* Let's add "TPE" later. */ },
            ["TPE"] = new() { "TPE" }
        };
        ListAdjacencyGraph<string, Dictionary<string, List<string>>> graph =
            ListAdjacencyGraphFactory<string>.Create(neighborsByVertex);
        graph.AddEdge("IST", "TPE");

        IncidenceEnumerator<string, List<string>.Enumerator> flightsFromIstanbul =
            graph.EnumerateOutEdges("IST");
        while (flightsFromIstanbul.MoveNext())
            Console.WriteLine(flightsFromIstanbul.Current);
    }

    private static void DemoIncidenceListGraph()
    {
        ListIncidenceGraph<string, int, Dictionary<int, string>, Dictionary<string, List<int>>> graph =
            ListIncidenceGraphFactory<string, int>.Create();
    }
}
