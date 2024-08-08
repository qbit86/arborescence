namespace Arborescence;

using System;
using System.Collections.Generic;
using Models.Specialized;

internal sealed class GraphHelper
{
    private Dictionary<int, Int32IncidenceGraph>? _graphCache;

    internal static GraphHelper Default { get; } = new();

    internal Int32IncidenceGraph GetIncidenceGraph(int vertexCount)
    {
        _graphCache ??= new();

        if (_graphCache.TryGetValue(vertexCount, out var result))
            return result;

        result = CreateIncidenceGraph(vertexCount);
        _graphCache[vertexCount] = result;
        return result;
    }

    private static Int32IncidenceGraph CreateIncidenceGraph(int vertexCount)
    {
        int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.5));

        Random prng = new(1729);
        List<Endpoints<int>> endpointsByEdge = new(vertexCount);

        for (int e = 0; e < edgeCount; ++e)
        {
            int tail = prng.Next(vertexCount);
            int head = prng.Next(vertexCount);
            endpointsByEdge.Add(new(tail, head));
        }

        return Int32IncidenceGraph.FromEdges(endpointsByEdge);
    }
}
