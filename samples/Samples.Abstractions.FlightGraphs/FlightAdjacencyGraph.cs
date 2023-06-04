namespace Arborescence;

using System;
using System.Collections.Generic;
using NeighborEnumerator = System.ArraySegment<string>.Enumerator;

public sealed class FlightAdjacencyGraph :
    IOutNeighborsAdjacency<string, NeighborEnumerator>
{
    private readonly Dictionary<string, string[]> _neighborsByAirport;

    private FlightAdjacencyGraph(
        Dictionary<string, string[]> neighborsByAirport) =>
        _neighborsByAirport = neighborsByAirport;

    public NeighborEnumerator EnumerateOutNeighbors(string vertex) =>
        _neighborsByAirport.TryGetValue(vertex, out string[]? neighbors)
            ? new ArraySegment<string>(neighbors).GetEnumerator()
            : ArraySegment<string>.Empty.GetEnumerator();

    public static FlightAdjacencyGraph Create()
    {
        Dictionary<string, string[]> neighborsByAirport = new(3)
        {
            { "LHR", new[] { "IST", "IST" } },
            { "IST", new[] { "LHR", "TPE" } },
            { "TPE", new[] { "TPE" } }
        };
        return new(neighborsByAirport);
    }
}
