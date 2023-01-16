using System;
using Arborescence;

var adjacencyGraph = FlightAdjacencyGraph.Create();
ArraySegment<string>.Enumerator istanbulNeighborEnumerator =
    adjacencyGraph.EnumerateNeighbors("IST");
while (istanbulNeighborEnumerator.MoveNext())
    Console.WriteLine(istanbulNeighborEnumerator.Current);

Console.WriteLine();

var incidenceGraph = FlightIncidenceGraph.Create();
ArraySegment<int>.Enumerator istanbulFlightEnumerator = incidenceGraph.EnumerateOutEdges("IST");
while (istanbulFlightEnumerator.MoveNext())
    Console.WriteLine(istanbulFlightEnumerator.Current);

Console.WriteLine();

if (incidenceGraph.TryGetTail(676, out string? flight676Origin))
    Console.WriteLine(flight676Origin);
if (incidenceGraph.TryGetHead(676, out string? flight676Destination))
    Console.WriteLine(flight676Destination);
