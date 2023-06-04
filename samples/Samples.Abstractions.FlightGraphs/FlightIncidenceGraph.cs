namespace Arborescence;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

public sealed class FlightIncidenceGraph :
    IGraph<string, int>, IForwardIncidence<string, int, EdgeEnumerator>
{
    private readonly Dictionary<int, string> _destinationByFlight;
    private readonly Dictionary<string, int[]> _flightsByOrigin;
    private readonly Dictionary<int, string> _originByFlight;

    private FlightIncidenceGraph(
        Dictionary<int, string> originByFlight,
        Dictionary<int, string> destinationByFlight,
        Dictionary<string, int[]> flightsByOrigin)
    {
        _originByFlight = originByFlight;
        _destinationByFlight = destinationByFlight;
        _flightsByOrigin = flightsByOrigin;
    }

    public EdgeEnumerator EnumerateOutEdges(string vertex) =>
        _flightsByOrigin.TryGetValue(vertex, out int[]? flights)
            ? new ArraySegment<int>(flights).GetEnumerator()
            : ArraySegment<int>.Empty.GetEnumerator();

    public bool TryGetTail(int edge, [MaybeNullWhen(false)] out string tail) =>
        _originByFlight.TryGetValue(edge, out tail);

    public bool TryGetHead(int edge, [MaybeNullWhen(false)] out string head) =>
        _destinationByFlight.TryGetValue(edge, out head);

    public static FlightIncidenceGraph Create()
    {
        Dictionary<int, string> originByFlight = new(5)
        {
            { 676, "LHR" }, { 1980, "LHR" }, { 677, "IST" }, { 24, "IST" }, { 5288, "TPE" }
        };
        Dictionary<int, string> destinationByFlight = new(5)
        {
            { 676, "IST" }, { 1980, "IST" }, { 677, "LHR" }, { 24, "TPE" }, { 5288, "TPE" }
        };
        Dictionary<string, int[]> flightsByOrigin = new(3)
        {
            { "LHR", new[] { 676, 1980 } },
            { "IST", new[] { 677, 24 } },
            { "TPE", new[] { 5288 } }
        };
        return new(originByFlight, destinationByFlight, flightsByOrigin);
    }
}
