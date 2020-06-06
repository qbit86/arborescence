namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Models;

    internal static class GraphHelper
    {
        internal static IEnumerable<object[]> CreateTestCases()
        {
            return Enumerable.Range(1, 7)
                .Select(it => new object[] { it.ToString("D2", CultureInfo.InvariantCulture) });
        }

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
    }
}
