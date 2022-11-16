namespace Arborescence;

using System;
using System.Collections.Generic;
using Models;

internal sealed class GraphHelper
{
    private Dictionary<int, IndexedIncidenceGraph>? _cache;

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
}