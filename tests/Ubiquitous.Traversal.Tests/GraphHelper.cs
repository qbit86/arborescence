namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Models;

    internal static class GraphHelper
    {
        private static readonly double[] s_densityPowers = { 1.0, Math.Sqrt(2.0), 0.5 * (1.0 + Math.Sqrt(5.0)), 2.0 };

        internal static IEnumerable<object[]> CreateGraphSizes()
        {
            yield return new object[] { 1, 1.0 };

            for (int power = 1; power < 7; ++power)
            {
                double exp = Math.Pow(10.0, 0.5 * power);
                int vertexCount = (int)Math.Ceiling(exp);
                foreach (double densityPower in s_densityPowers)
                {
                    yield return new object[] { vertexCount, densityPower };
                }
            }
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
