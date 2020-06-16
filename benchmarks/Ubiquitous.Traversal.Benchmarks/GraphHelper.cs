namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Models;

    internal struct GraphHelper
    {
        private Dictionary<int, AdjacencyListIncidenceGraph> _cache;

        internal static GraphHelper Default { get; } = new GraphHelper();

        internal AdjacencyListIncidenceGraph GetGraph(int vertexCount)
        {
            _cache ??= new Dictionary<int, AdjacencyListIncidenceGraph>();

            if (_cache.TryGetValue(vertexCount, out AdjacencyListIncidenceGraph result))
                return result;

            result = CreateGraph(vertexCount);
            _cache[vertexCount] = result;
            return result;
        }

        private static AdjacencyListIncidenceGraph CreateGraph(int vertexCount)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.618));

            var builder = new AdjacencyListIncidenceGraphBuilder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                builder.TryAdd(tail, head, out _);
            }

            return builder.ToGraph();
        }
    }
}
