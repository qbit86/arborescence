namespace Arborescence;

using System;
using System.Collections.Generic;
using NeighborEnumerator = System.ArraySegment<string>.Enumerator;

public sealed class FlightAdjacencyGraph :
    IOutNeighborsAdjacency<string, NeighborEnumerator>
{
    private static readonly string[] s_lhrNeighbors = { "IST", "IST" };
    private static readonly string[] s_istNeighbors = { "LHR", "TPE" };
    private static readonly string[] s_tpeNeighbors = { "TPE" };
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
            { "LHR", s_lhrNeighbors },
            { "IST", s_istNeighbors },
            { "TPE", s_tpeNeighbors }
        };
        return new(neighborsByAirport);
    }
}
