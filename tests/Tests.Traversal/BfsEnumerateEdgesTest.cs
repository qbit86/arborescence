namespace Arborescence.Traversal.Bfs;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Incidence;
using Misnomer;
using Xunit;
using Graph = Models.Specialized.Int32AdjacencyGraph;
using EdgeEnumerator = IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

public sealed class BfsEnumerateEdgesTest
{
    private static void EnumerateEdgesCore(Models.Specialized.Int32AdjacencyGraph graph, bool multipleSource)
    {
        // Arrange

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        Int32ColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet set = new(setBackingStore);

        using Rist<Endpoints<int>> eagerSteps = new(graph.VertexCount);
        using Rist<Endpoints<int>> enumerableSteps = new(graph.VertexCount);
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
            IEnumerable<Endpoints<int>> edges = EnumerableBfs<int, Endpoints<int>, EdgeEnumerator>.EnumerateEdges(
                graph, sources, set);
            // ReSharper restore PossibleMultipleEnumeration
            enumerableSteps.AddRange(edges);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerBfs<int, Endpoints<int>, EdgeEnumerator>.Traverse(graph, source, eagerColorByVertex, bfsHandler);
            IEnumerable<Endpoints<int>> edges = EnumerableBfs<int, Endpoints<int>, EdgeEnumerator>.EnumerateEdges(
                graph, source, set);
            enumerableSteps.AddRange(edges);
        }

        // Assert

        int eagerStepCount = eagerSteps.Count;
        int enumerableStepCount = enumerableSteps.Count;
        Assert.Equal(eagerStepCount, enumerableStepCount);

        int count = eagerStepCount;
        for (int i = 0; i < count; ++i)
        {
            Endpoints<int> eagerStep = eagerSteps[i];
            Endpoints<int> enumerableStep = enumerableSteps[i];

            if (eagerStep == enumerableStep)
                continue;

            Assert.Equal(eagerStep, enumerableStep);
        }

        // Cleanup

        ArrayPool<byte>.Shared.Return(colorByVertexBackingStore);
        ArrayPool<byte>.Shared.Return(setBackingStore);
    }

    private static BfsHandler<int, Endpoints<int>, Graph> CreateBfsHandler(IList<Endpoints<int>> treeEdges)
    {
        if (treeEdges is null)
            throw new ArgumentNullException(nameof(treeEdges));

        BfsHandler<int, Endpoints<int>, Graph> result = new();
        result.TreeEdge += (_, e) => treeEdges.Add(e);
        return result;
    }

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, true);
}
