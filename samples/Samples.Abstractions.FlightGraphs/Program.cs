using System;
using Arborescence;
using NeighborEnumerator = System.ArraySegment<string>.Enumerator;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

var adjacencyGraph = FlightAdjacencyGraph.Create();
NeighborEnumerator istanbulNeighborEnumerator =
    adjacencyGraph.EnumerateOutNeighbors("IST");
while (istanbulNeighborEnumerator.MoveNext())
    Console.WriteLine(istanbulNeighborEnumerator.Current);

Console.WriteLine();

var incidenceGraph = FlightIncidenceGraph.Create();
EdgeEnumerator istanbulFlightEnumerator =
    incidenceGraph.EnumerateOutEdges("IST");
while (istanbulFlightEnumerator.MoveNext())
    Console.WriteLine(istanbulFlightEnumerator.Current);

Console.WriteLine();

if (incidenceGraph.TryGetTail(676, out string? flight676Origin))
    Console.WriteLine(flight676Origin);
if (incidenceGraph.TryGetHead(676, out string? flight676Destination))
    Console.WriteLine(flight676Destination);
