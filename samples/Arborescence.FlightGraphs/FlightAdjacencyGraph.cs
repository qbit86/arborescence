namespace Arborescence;

using System;
using System.Collections.Generic;

public sealed class FlightAdjacencyGraph : IAdjacency<string, ArraySegment<string>.Enumerator>
{
    private readonly Dictionary<string, string[]> _neighborsByAirport;

    private FlightAdjacencyGraph(Dictionary<string, string[]> neighborsByAirport) =>
        _neighborsByAirport = neighborsByAirport;

    public ArraySegment<string>.Enumerator EnumerateNeighbors(string vertex) =>
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
