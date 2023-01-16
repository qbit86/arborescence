using System;
using Arborescence;

var adjacencyGraph = FlightAdjacencyGraph.Create();
ArraySegment<string>.Enumerator istanbulNeighborEnumerator =
    adjacencyGraph.EnumerateNeighbors("IST");
while (istanbulNeighborEnumerator.MoveNext())
    Console.WriteLine(istanbulNeighborEnumerator.Current);
