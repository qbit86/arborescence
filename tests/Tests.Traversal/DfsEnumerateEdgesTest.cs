namespace Arborescence.Traversal.Dfs;

using System;
using System.Buffers;
using System.Collections.Generic;
using Incidence;
using Misnomer;
using Xunit;
using Graph = Models.Specialized.Int32IncidenceGraph;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
using EnumerableDfs = Incidence.EnumerableDfs<int, int, System.ArraySegment<int>.Enumerator>;

public sealed class DfsEnumerateEdgesTest
{
    private static void EnumerateEdgesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        Int32ColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
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
            IEnumerable<int> edges = EnumerableDfs.EnumerateEdges(graph, sources, set);
            enumerableSteps.AddRange(edges);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerDfs<int, int, EdgeEnumerator>.Traverse(graph, source, eagerColorByVertex, dfsHandler);
            IEnumerable<int> edges = EnumerableDfs.EnumerateEdges(graph, source, set);
            enumerableSteps.AddRange(edges);
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

    private static DfsHandler<int, int, Graph> CreateDfsHandler(IList<int> treeEdges)
    {
        if (treeEdges is null)
            throw new ArgumentNullException(nameof(treeEdges));

        DfsHandler<int, int, Graph> result = new();
        result.TreeEdge += (_, e) => treeEdges.Add(e);
        return result;
    }

    [Theory]
    [ClassData(typeof(Int32IncidenceGraphCollection))]
    internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(Int32IncidenceGraphCollection))]
    internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, true);
}
