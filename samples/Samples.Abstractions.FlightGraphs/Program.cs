using System;
using Arborescence;

var adjacencyGraph = FlightAdjacencyGraph.Create();
var istanbulNeighborEnumerator =
    adjacencyGraph.EnumerateOutNeighbors("IST");
while (istanbulNeighborEnumerator.MoveNext())
    Console.WriteLine(istanbulNeighborEnumerator.Current);

Console.WriteLine();

var incidenceGraph = FlightIncidenceGraph.Create();
var istanbulFlightEnumerator =
    incidenceGraph.EnumerateOutEdges("IST");
while (istanbulFlightEnumerator.MoveNext())
    Console.WriteLine(istanbulFlightEnumerator.Current);

Console.WriteLine();

if (incidenceGraph.TryGetTail(676, out string? flight676Origin))
    Console.WriteLine(flight676Origin);
if (incidenceGraph.TryGetHead(676, out string? flight676Destination))
    Console.WriteLine(flight676Destination);
