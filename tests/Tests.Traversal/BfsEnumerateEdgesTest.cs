namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Misnomer;
using Traversal;
using Traversal.Incidence;
using Xunit;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using Graph = Models.SimpleIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;
#else
using Graph = Models.Compatibility.SimpleIncidenceGraph;
using EdgeEnumerator = System.Collections.Generic.IEnumerator<Int32Endpoints>;
#endif

public sealed class BfsEnumerateEdgesTest
{
    private static void EnumerateEdgesCore(Graph graph, bool multipleSource)
    {
        // Arrange

        byte[] colorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(colorByVertexBackingStore, 0, colorByVertexBackingStore.Length);
        IndexedColorDictionary eagerColorByVertex = new(colorByVertexBackingStore);
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet set = new(setBackingStore);

        using Rist<Int32Endpoints> eagerSteps = new(graph.VertexCount);
        using Rist<Int32Endpoints> enumerableSteps = new(graph.VertexCount);
        BfsHandler<int, Int32Endpoints, Graph> bfsHandler = CreateBfsHandler(eagerSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IEnumerable<int> sources = Enumerable.Range(0, sourceCount);

            // ReSharper disable PossibleMultipleEnumeration
            EagerBfs<int, Int32Endpoints, EdgeEnumerator>.Traverse(graph, sources, eagerColorByVertex, bfsHandler);
            IEnumerable<Int32Endpoints> edges = EnumerableBfs<int, Int32Endpoints, EdgeEnumerator>.EnumerateEdges(
                graph, sources, set);
            // ReSharper restore PossibleMultipleEnumeration
            enumerableSteps.AddRange(edges);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerBfs<int, Int32Endpoints, EdgeEnumerator>.Traverse(graph, source, eagerColorByVertex, bfsHandler);
            IEnumerable<Int32Endpoints> edges = EnumerableBfs<int, Int32Endpoints, EdgeEnumerator>.EnumerateEdges(
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
            Int32Endpoints eagerStep = eagerSteps[i];
            Int32Endpoints enumerableStep = enumerableSteps[i];

            if (eagerStep == enumerableStep)
                continue;

            Assert.Equal(eagerStep, enumerableStep);
        }

        // Cleanup

        ArrayPool<byte>.Shared.Return(colorByVertexBackingStore);
        ArrayPool<byte>.Shared.Return(setBackingStore);
    }

    private static BfsHandler<int, Int32Endpoints, Graph> CreateBfsHandler(IList<Int32Endpoints> treeEdges)
    {
        if (treeEdges is null)
            throw new ArgumentNullException(nameof(treeEdges));

        BfsHandler<int, Int32Endpoints, Graph> result = new();
        result.TreeEdge += (_, e) => treeEdges.Add(e);
        return result;
    }

    [Theory]
    [ClassData(typeof(UndirectedSimpleGraphCollection))]
    internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(SimpleGraphCollection))]
    internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p) => EnumerateEdgesCore(p.Graph, true);
}
