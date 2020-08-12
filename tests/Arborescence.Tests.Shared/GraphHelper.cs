namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Models;

    internal static class GraphHelper
    {
        internal static double[] DensityPowers { get; } = { 1.0, 1.5, 2.0 };

        internal static void PopulateIncidenceGraphBuilder<TGraph, TEdge, TEdges, TGraphBuilder>(
            TGraphBuilder builder, int vertexCount, double densityPower)
            where TGraph : IIncidenceGraph<int, TEdge, TEdges>
            where TGraphBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));

            var prng = new Random(1729);
            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                builder.TryAdd(tail, head, out _);
            }
        }

        internal static void GenerateEdges(int vertexCount, double densityPower, IList<Endpoints> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));
            if (edges is List<Endpoints> list)
                list.Capacity = edgeCount;

            var prng = new Random(1729);
            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                edges.Add(new Endpoints(tail, head));
            }
        }
    }
}
