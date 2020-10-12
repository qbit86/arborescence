namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Models;

    internal sealed class GraphHelper
    {
        private Dictionary<int, IndexedIncidenceGraph>? _cache;

        internal static GraphHelper Default { get; } = new GraphHelper();

        internal IndexedIncidenceGraph GetGraph(int vertexCount)
        {
            _cache ??= new Dictionary<int, IndexedIncidenceGraph>();

            if (_cache.TryGetValue(vertexCount, out IndexedIncidenceGraph result))
                return result;

            result = CreateGraph(vertexCount);
            _cache[vertexCount] = result;
            return result;
        }

        private static IndexedIncidenceGraph CreateGraph(int vertexCount)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.5));

            var builder = new IndexedIncidenceGraph.Builder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                builder.TryAdd(tail, head);
            }

            return builder.ToGraph();
        }
    }
}
