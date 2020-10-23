namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Models;
    using Traversal;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

    internal static partial class Program
    {
        // ReSharper disable once UnusedMember.Local
        private static void DemoRecursiveDfs()
        {
            const int vertexCount = 10000;
            const double densityPower = 1.5;
            IndexedIncidenceGraph graph = GenerateIncidenceGraph(vertexCount, densityPower);

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var steps = new List<int>();
            DfsHandler<IndexedIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(steps);
            RecursiveDfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedIncidenceGraphPolicy, IndexedColorMapPolicy> dfs = default;

            dfs.Traverse(graph, 0, colorMap, dfsHandler);
            Console.WriteLine(steps.Count);

            ArrayPool<byte>.Shared.Return(colorMap, true);
        }

        private static DfsHandler<IndexedIncidenceGraph, int, int> CreateDfsHandler(IList<int> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            var result = new DfsHandler<IndexedIncidenceGraph, int, int>();
            result.TreeEdge += (g, e) => steps.Add(e);
            return result;
        }

        private static IndexedIncidenceGraph GenerateIncidenceGraph(int vertexCount, double densityPower)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));

            var builder = new IndexedIncidenceGraph.Builder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int tail = prng.Next(vertexCount);
                int head = prng.Next(vertexCount);
                builder.Add(tail, head);
            }

            IndexedIncidenceGraph result = builder.ToGraph();
            return result;
        }
    }
}
