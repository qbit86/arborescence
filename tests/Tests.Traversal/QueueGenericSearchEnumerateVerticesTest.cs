namespace Arborescence.Traversal.Generic;

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Incidence;
using Misnomer;
using Xunit;
using Graph = Models.ListAdjacencyGraph<
    int,
    Int32Dictionary<
        System.Collections.Generic.List<int>,
        System.Collections.Generic.List<System.Collections.Generic.List<int>>>>;
using EdgeEnumerator = IncidenceEnumerator<int, System.Collections.Generic.List<int>.Enumerator>;

public class QueueGenericSearchEnumerateVerticesTest
{
    private static void EnumerateVerticesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        Int32ColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
        ConcurrentQueue<int> frontier = new();
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        Int32Set set = new(setBackingStore);

        using Rist<int> eagerSteps = new(graph.VertexCount);
        using Rist<int> enumerableSteps = new(graph.VertexCount);
        BfsHandler<int, Endpoints<int>, Graph> bfsHandler = CreateBfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IEnumerable<int> sources = Enumerable.Range(0, sourceCount);

            // ReSharper disable PossibleMultipleEnumeration
            EagerBfs<int, Endpoints<int>, EdgeEnumerator>.Traverse(graph, sources, eagerColorByVertex, bfsHandler);
            IEnumerable<int> vertices = EnumerableGenericSearch<int, Endpoints<int>, EdgeEnumerator>.EnumerateVertices(
                graph, sources, frontier, set);
            // ReSharper restore PossibleMultipleEnumeration
            enumerableSteps.AddRange(vertices);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerBfs<int, Endpoints<int>, EdgeEnumerator>.Traverse(graph, source, eagerColorByVertex, bfsHandler);
            IEnumerable<int> vertices = EnumerableGenericSearch<int, Endpoints<int>, EdgeEnumerator>.EnumerateVertices(
                graph, source, frontier, set);
            enumerableSteps.AddRange(vertices);
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

    private static BfsHandler<int, Endpoints<int>, Graph> CreateBfsHandler(IList<int> discoveredVertices)
    {
        if (discoveredVertices is null)
            throw new ArgumentNullException(nameof(discoveredVertices));

        BfsHandler<int, Endpoints<int>, Graph> result = new();
        result.DiscoverVertex += (_, v) => discoveredVertices.Add(v);
        return result;
    }

    [Theory]
    [ClassData(typeof(ListAdjacencyGraphCollection))]
    internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(ListAdjacencyGraphCollection))]
    internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, true);
}
