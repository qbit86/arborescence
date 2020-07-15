namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Models;

    internal static class GraphHelper
    {
        internal static AdjacencyListIncidenceGraph GenerateAdjacencyListIncidenceGraph(
            int vertexCount, double densityPower)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));

            var builder = new AdjacencyListIncidenceGraphBuilder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                builder.TryAdd(tail, head, out _);
            }

            AdjacencyListIncidenceGraph result = builder.ToGraph();
            return result;
        }

        internal static void GenerateEdges(int vertexCount, double densityPower, IList<Endpoints<int>> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));
            if (edges is List<Endpoints<int>> list)
                list.Capacity = edgeCount;

            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                edges.Add(Endpoints.Create(tail, head));
            }
        }
    }
}
