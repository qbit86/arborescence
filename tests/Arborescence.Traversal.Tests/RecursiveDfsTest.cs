namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Misnomer;
using Traversal;
using Traversal.Incidence;
using Xunit;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using Graph = Models.MutableIndexedIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
#else
using Graph = Models.Compatibility.MutableIndexedIncidenceGraph;
using EdgeEnumerator = System.Collections.Generic.IEnumerator<int>;
#endif

public sealed class RecursiveDfsTest
{
    private void TraverseCore(Graph graph, bool multipleSource)
    {
        // Arrange

        byte[] eagerColorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(eagerColorByVertexBackingStore, 0, eagerColorByVertexBackingStore.Length);
        IndexedColorDictionary eagerColorByVertex = new(eagerColorByVertexBackingStore);
        using Rist<(string, int)> eagerSteps = new(Math.Max(graph.VertexCount, 1));
        DfsHandler<int, int, Graph> eagerHandler = CreateDfsHandler(eagerSteps);

        byte[] recursiveColorByVertexBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
        Array.Clear(recursiveColorByVertexBackingStore, 0, recursiveColorByVertexBackingStore.Length);
        IndexedColorDictionary recursiveColorByVertex = new(recursiveColorByVertexBackingStore);
        using Rist<(string, int)> recursiveSteps = new(Math.Max(graph.VertexCount, 1));
        DfsHandler<int, int, Graph> recursiveHandler = CreateDfsHandler(recursiveSteps);

        // Act

        if (multipleSource)
        {
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            IndexEnumerator sources = new(sourceCount);

            EagerDfs<int, int, EdgeEnumerator>.Traverse(graph, sources, eagerColorByVertex, eagerHandler);
            RecursiveDfs<int, int, EdgeEnumerator>.Traverse(graph, sources, recursiveColorByVertex, recursiveHandler);
        }
        else
        {
            int source = graph.VertexCount >> 1;
            EagerDfs<int, int, EdgeEnumerator>.Traverse(graph, source, eagerColorByVertex, eagerHandler);
            RecursiveDfs<int, int, EdgeEnumerator>.Traverse(graph, source, recursiveColorByVertex, recursiveHandler);
        }

        // Assert

        int eagerStepCount = eagerSteps.Count;
        int recursiveStepCount = recursiveSteps.Count;
        Assert.Equal(eagerStepCount, recursiveStepCount);

        int count = eagerStepCount;
        for (int i = 0; i < count; ++i)
        {
            (string, int) eagerStep = eagerSteps[i];
            (string, int) recursiveStep = recursiveSteps[i];

            if (eagerStep == recursiveStep)
                continue;

            Assert.Equal(eagerStep, recursiveStep);
        }

        // Cleanup

        ArrayPool<byte>.Shared.Return(eagerColorByVertexBackingStore);
        ArrayPool<byte>.Shared.Return(recursiveColorByVertexBackingStore);
    }

    private static DfsHandler<int, int, Graph> CreateDfsHandler(ICollection<(string, int)> steps)
    {
        DfsHandler<int, int, Graph> result = new();
        result.StartVertex += (_, v) => steps.Add((nameof(result.OnStartVertex), v));
        result.DiscoverVertex += (_, v) => steps.Add((nameof(result.DiscoverVertex), v));
        result.FinishVertex += (_, v) => steps.Add((nameof(result.FinishVertex), v));
        result.TreeEdge += (_, e) => steps.Add((nameof(result.TreeEdge), e));
        result.BackEdge += (_, e) => steps.Add((nameof(result.BackEdge), e));
        result.ExamineEdge += (_, e) => steps.Add((nameof(result.ExamineEdge), e));
        result.ForwardOrCrossEdge += (_, e) => steps.Add((nameof(result.ForwardOrCrossEdge), e));
        result.FinishEdge += (_, e) => steps.Add((nameof(result.FinishEdge), e));
        return result;
    }

    [Theory]
    [ClassData(typeof(MutableIndexedGraphCollection))]
    internal void Traverse_SingleSource(GraphParameter<Graph> p) => TraverseCore(p.Graph, false);

    [Theory]
    [ClassData(typeof(MutableIndexedGraphCollection))]
    internal void Traverse_MultipleSource(GraphParameter<Graph> p) => TraverseCore(p.Graph, true);
}
