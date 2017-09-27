namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    internal struct GraphHelper
    {
        private Dictionary<int, IndexedAdjacencyListGraph> _cache;

        internal static GraphHelper Default { get; } = new GraphHelper();

        internal IndexedAdjacencyListGraph GetGraph(int vertexCount)
        {
            if (_cache == null)
                _cache = new Dictionary<int, IndexedAdjacencyListGraph>();

            IndexedAdjacencyListGraph result;
            if (_cache.TryGetValue(vertexCount, out result))
                return result;

            result = CreateGraph(vertexCount);
            _cache[vertexCount] = result;
            return result;
        }

        internal static IndexedAdjacencyListGraph CreateGraph(int vertexCount)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.618));

            var builder = new IndexedAdjacencyListGraphBuilder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexCount);
                int target = prng.Next(vertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }

            return builder.MoveToIndexedAdjacencyListGraph();
        }
    }
}
