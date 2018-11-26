namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Models;

    internal struct GraphHelper
    {
        private Dictionary<int, JaggedAdjacencyListIncidenceGraph> _cache;

        internal static GraphHelper Default { get; } = new GraphHelper();

        internal JaggedAdjacencyListIncidenceGraph GetGraph(int vertexUpperBound)
        {
            if (_cache == null)
                _cache = new Dictionary<int, JaggedAdjacencyListIncidenceGraph>();

            if (_cache.TryGetValue(vertexUpperBound, out JaggedAdjacencyListIncidenceGraph result))
                return result;

            result = CreateGraph(vertexUpperBound);
            _cache[vertexUpperBound] = result;
            return result;
        }

        private static JaggedAdjacencyListIncidenceGraph CreateGraph(int vertexUpperBound)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexUpperBound, 1.618));

            var builder = new JaggedAdjacencyListIncidenceGraphBuilder(vertexUpperBound);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexUpperBound);
                int target = prng.Next(vertexUpperBound);
                builder.TryAdd(source, target, out _);
            }

            return builder.ToGraph();
        }
    }
}
