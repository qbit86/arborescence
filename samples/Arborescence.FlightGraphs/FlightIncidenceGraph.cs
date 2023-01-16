namespace Arborescence;

using System;
using System.Diagnostics.CodeAnalysis;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

public sealed class FlightIncidenceGraph :
    IGraph<string, int>, IForwardIncidence<string, int, EdgeEnumerator>
{
    public EdgeEnumerator EnumerateOutEdges(string vertex) => throw new NotImplementedException();

    public bool TryGetTail(int edge, [UnscopedRef] out string tail) => throw new NotImplementedException();

    public bool TryGetHead(int edge, [UnscopedRef] out string head) => throw new NotImplementedException();
}
