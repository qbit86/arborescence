namespace Arborescence;

using System;
using System.Collections.Generic;
using Models;
using Models.Specialized;

internal sealed class GraphHelper
{
    private Dictionary<int, IndexedIncidenceGraph>? _cache;
    private Dictionary<int, Int32IncidenceGraph>? _graphCache;

    internal static GraphHelper Default { get; } = new();

    internal IndexedIncidenceGraph GetGraph(int vertexCount)
    {
        _cache ??= new();

        if (_cache.TryGetValue(vertexCount, out IndexedIncidenceGraph result))
            return result;

        result = CreateGraph(vertexCount);
        _cache[vertexCount] = result;
        return result;
    }

    internal Int32IncidenceGraph GetIncidenceGraph(int vertexCount)
    {
        _graphCache ??= new();

        if (_graphCache.TryGetValue(vertexCount, out Int32IncidenceGraph result))
            return result;

        result = CreateIncidenceGraph(vertexCount);
        _graphCache[vertexCount] = result;
        return result;
    }

    private static IndexedIncidenceGraph CreateGraph(int vertexCount)
    {
        int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.5));

        IndexedIncidenceGraph.Builder builder = new(vertexCount);
        Random prng = new(1729);

        for (int e = 0; e < edgeCount; ++e)
        {
            int tail = prng.Next(vertexCount);
            int head = prng.Next(vertexCount);
            builder.Add(tail, head);
        }

        return builder.ToGraph();
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

        return Int32IncidenceGraphFactory.FromEdges(endpointsByEdge);
    }
}
