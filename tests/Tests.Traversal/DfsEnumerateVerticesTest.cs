namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Misnomer;
using Traversal;
using Traversal.Incidence;
using Xunit;
using Graph = Models.ListIncidenceGraph<
    int,
    int,
    Int32Dictionary<int, System.Collections.Generic.List<int>>,
    System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>>>;
using EdgeEnumerator = System.Collections.Generic.List<int>.Enumerator;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<int, int, System.Collections.Generic.List<int>.Enumerator>;

public sealed class DfsEnumerateVerticesTest
{
    private static void EnumerateVerticesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        if (graph.VertexCount == 0)
            return;

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        IndexedColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet set = new(setBackingStore);

        using Rist<int> eagerSteps = new(graph.VertexCount);
        using Rist<int> enumerableSteps = new(graph.VertexCount);
        DfsHandler<int, int, Graph> dfsHandler = CreateDfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IndexEnumerator sources = new(sourceCount);
            EagerDfs<int, int, EdgeEnumerator>.Traverse(graph, sources, eagerColorByVertex, dfsHandler);
            IEnumerable<int> vertices = EnumerableDfs.EnumerateVertices(graph, sources, set);
            enumerableSteps.AddRange(vertices);
        }
        else
        {
            int source = graph.VertexCount - 1;
            EagerDfs<int, int, EdgeEnumerator>.Traverse(graph, source, eagerColorByVertex, dfsHandler);
            IEnumerable<int> vertices = EnumerableDfs.EnumerateVertices(graph, source, set);
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

    private static DfsHandler<int, int, Graph> CreateDfsHandler(IList<int> steps)
    {
        if (steps is null)
            throw new ArgumentNullException(nameof(steps));

        DfsHandler<int, int, Graph> result = new();
        result.DiscoverVertex += (_, v) => steps.Add(v);
        return result;
    }

    [Theory]
    [ClassData(typeof(ListIncidenceGraphCollection))]
    internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(ListIncidenceGraphCollection))]
    internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p) => EnumerateVerticesCore(p.Graph, true);
}
