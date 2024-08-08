namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Models.Specialized;
using Traversal;
using Traversal.Incidence;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

internal static partial class Program
{
    private static void DemoRecursiveDfs()
    {
        const int vertexCount = 1000;
        const double densityPower = 1.5;
        var graph = GenerateIncidenceGraph(vertexCount, densityPower);

        byte[] backingStore = ArrayPool<byte>.Shared.Rent(vertexCount);
        Array.Clear(backingStore, 0, backingStore.Length);
        Int32ColorDictionary colorByVertex = new(backingStore);
        List<int> steps = new();
        var dfsHandler = CreateDfsHandler(steps);

        RecursiveDfs<int, int, EdgeEnumerator>.Traverse(graph, 0, colorByVertex, dfsHandler);
        Console.WriteLine(steps.Count);

        ArrayPool<byte>.Shared.Return(backingStore, true);
    }

    private static DfsHandler<int, int, Int32IncidenceGraph> CreateDfsHandler<TCollection>(TCollection steps)
        where TCollection : ICollection<int>
    {
        DfsHandler<int, int, Int32IncidenceGraph> result = new();
        result.TreeEdge += (_, e) => steps.Add(e);
        return result;
    }

    private static Int32IncidenceGraph GenerateIncidenceGraph(int vertexCount, double densityPower)
    {
        int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));

        List<Endpoints<int>> endpointsByEdge = new(vertexCount);
        Random prng = new(1729);

        for (int e = 0; e < edgeCount; ++e)
        {
            int tail = prng.Next(vertexCount);
            int head = prng.Next(vertexCount);
            endpointsByEdge.Add(new(tail, head));
        }

        return Int32IncidenceGraph.FromEdges(endpointsByEdge);
    }
}
