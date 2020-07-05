﻿namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Models;
    using Traversal;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal static partial class Program
    {
        private static void DemoRecursiveDfs()
        {
            const int vertexCount = 10000;
            const double densityPower = 1.5;
            AdjacencyListIncidenceGraph graph = GenerateAdjacencyListIncidenceGraph(vertexCount, densityPower);

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var steps = new List<int>();
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(steps);
            RecursiveDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy> dfs = default;

            dfs.Traverse(graph, 0, colorMap, dfsHandler);
            Console.WriteLine(steps.Count);

            ArrayPool<byte>.Shared.Return(colorMap, true);
        }

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(IList<int> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.TreeEdge += (g, e) => steps.Add(e);
            return result;
        }

        private static AdjacencyListIncidenceGraph GenerateAdjacencyListIncidenceGraph(
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