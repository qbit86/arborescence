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
            ["IST"] = new() { "LHR" }
            // Let's add "TPE" later.
        };
        var graph = ListAdjacencyGraph<string>.Create(neighborsByVertex);
        graph.TryAddVertex("TPE");
        graph.AddEdge("IST", "TPE");
        graph.AddEdge("TPE", "TPE");

        var flightsFromIstanbul = graph.EnumerateOutEdges("IST");
        while (flightsFromIstanbul.MoveNext())
            Console.WriteLine(flightsFromIstanbul.Current);
    }

    private static void DemoIncidenceListGraph()
    {
        var graph = ListIncidenceGraph<string, int>.Create();
        _ = graph.TryAddVertex("OMS");
        _ = graph.TryAddEdge(676, "LHR", "IST");
        _ = graph.TryAddEdge(1980, "LHR", "IST");
        _ = graph.TryAddEdge(677, "IST", "LHR");
        _ = graph.TryAddEdge(24, "IST", "TPE");
        _ = graph.TryAddEdge(5288, "TPE", "TPE");

        var flightsFromIstanbul = graph.EnumerateOutEdges("IST");
        while (flightsFromIstanbul.MoveNext())
            Console.WriteLine(flightsFromIstanbul.Current);
    }
}
