namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Misnomer;
using Traversal;
using Traversal.Incidence;
using Xunit;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using Graph = Models.IndexedIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<int, int, System.ArraySegment<int>.Enumerator>;
#else
using Graph = Models.Compatibility.IndexedIncidenceGraph;
using EdgeEnumerator = System.Collections.Generic.IEnumerator<int>;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<int, int, System.Collections.Generic.IEnumerator<int>>;
#endif

public sealed class DfsEnumerateEdgesTest
{
    private void EnumerateEdgesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        IndexedColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
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
    [ClassData(typeof(IndexedGraphCollection))]
    internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(IndexedGraphCollection))]
    internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, true);
}
