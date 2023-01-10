namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Misnomer;
using Traversal;
using Xunit;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using Graph = Models.MutableSimpleIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<Endpoints>.Enumerator;
#else
using Graph = Models.Compatibility.MutableSimpleIncidenceGraph;
using EdgeEnumerator = System.Collections.Generic.IEnumerator<Endpoints>;
#endif

public class QueueGenericSearchEnumerateVerticesTest
{
    private EagerBfs<Graph, int, Endpoints, EdgeEnumerator> EagerBfs { get; }

    private GenericSearch<Graph, int, Endpoints, EdgeEnumerator> GenericSearch { get; }

    private void EnumerateVerticesCore(Graph graph, bool multipleSource)
    {
        Debug.Assert(graph != null, "graph != null");

        // Arrange

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph!.VertexCount, 1));
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        IndexedColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
        ConcurrentQueue<int> frontier = new();
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet set = new(setBackingStore);

        using Rist<int> eagerSteps = new(graph.VertexCount);
        using Rist<int> enumerableSteps = new(graph.VertexCount);
        BfsHandler<Graph, int, Endpoints> bfsHandler = CreateBfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IndexEnumerator sources = new(sourceCount);

            EagerBfs.Traverse(graph, sources, eagerColorByVertex, bfsHandler);
            using IEnumerator<int> vertices = GenericSearch.EnumerateVertices(graph, sources, frontier, set);
            enumerableSteps.AddEnumerator(vertices);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerBfs.Traverse(graph, source, eagerColorByVertex, bfsHandler);
            using IEnumerator<int> vertices = GenericSearch.EnumerateVertices(graph, source, frontier, set);
            enumerableSteps.AddEnumerator(vertices);
        }

        // Assert

        int eagerStepCount = eagerSteps.Count;
        int enumerableStepCount = enumerableSteps.Count;
        Assert.Equal(eagerStepCount, enumerableStepCount);

        int count = eagerStepCount;
        for (int i = 0; i < count; ++i)
        {
            int eagerStep = eagerSteps[i];
            int enumerableStep = enumerableSteps[i];

            if (eagerStep == enumerableStep)
                continue;

            Assert.Equal(eagerStep, enumerableStep);
        }

        // Cleanup

        ArrayPool<byte>.Shared.Return(colorByVertexBackingStore);
        ArrayPool<byte>.Shared.Return(setBackingStore);
    }

    private static BfsHandler<Graph, int, Endpoints> CreateBfsHandler(IList<int> discoveredVertices)
    {
        Debug.Assert(discoveredVertices != null, "discoveredVertices != null");

        BfsHandler<Graph, int, Endpoints> result = new();
        result.DiscoverVertex += (_, v) => discoveredVertices!.Add(v);
        return result;
    }

    [Theory]
    [ClassData(typeof(MutableSimpleGraphCollection))]
    internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(MutableSimpleGraphCollection))]
    internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, true);
}
