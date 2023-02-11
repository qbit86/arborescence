namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Models;
using Traversal;
using Traversal.Incidence;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

internal static partial class Program
{
    private static void DemoRecursiveDfs()
    {
        const int vertexCount = 10000;
        const double densityPower = 1.5;
        IndexedIncidenceGraph graph = GenerateIncidenceGraph(vertexCount, densityPower);

        byte[] backingStore = ArrayPool<byte>.Shared.Rent(vertexCount);
        Array.Clear(backingStore, 0, backingStore.Length);
        IndexedColorDictionary colorByVertex = new(backingStore);
        List<int> steps = new();
        DfsHandler<int, int, IndexedIncidenceGraph> dfsHandler = CreateDfsHandler(steps);

        RecursiveDfs<int, int, EdgeEnumerator>.Traverse(graph, 0, colorByVertex, dfsHandler);
        Console.WriteLine(steps.Count);

        ArrayPool<byte>.Shared.Return(backingStore, true);
    }

    private static DfsHandler<int, int, IndexedIncidenceGraph> CreateDfsHandler(ICollection<int> steps)
    {
        DfsHandler<int, int, IndexedIncidenceGraph> result = new();
        result.TreeEdge += (_, e) => steps.Add(e);
        return result;
    }

    private static IndexedIncidenceGraph GenerateIncidenceGraph(int vertexCount, double densityPower)
    {
        int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));

        IndexedIncidenceGraph.Builder builder = new(vertexCount);
        Random prng = new(1729);

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
