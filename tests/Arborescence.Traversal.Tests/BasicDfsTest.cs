namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Traversal.Specialized;
using Xunit;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using Graph = Models.SimpleIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;
using EnumerableDfs =
    Traversal.Incidence.EnumerableDfs<int, Int32Endpoints, System.ArraySegment<Int32Endpoints>.Enumerator>;
#else
using Graph = Models.Compatibility.SimpleIncidenceGraph;
using EdgeEnumerator = System.Collections.Generic.IEnumerator<Int32Endpoints>;
using EnumerableDfs =
    Traversal.Incidence.EnumerableDfs<int, Int32Endpoints, System.Collections.Generic.IEnumerator<Int32Endpoints>>;
#endif

public sealed class BasicDfsTest
{
    private EnumerableDfs<Graph, Int32Endpoints, EdgeEnumerator> Dfs { get; }

    [Theory]
    [ClassData(typeof(UndirectedSimpleGraphCollection))]
    internal void EnumerateEdges(GraphParameter<Graph> p)
    {
        Graph graph = p.Graph;

        // Arrange

        int source = graph.VertexCount >> 1;
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet exploredSet = new(setBackingStore);

        // Act

        using IEnumerator<Int32Endpoints> basicSteps = Dfs.EnumerateEdges(graph, source, graph.VertexCount);
        using IEnumerator<Int32Endpoints> enumerableSteps = EnumerableDfs.EnumerateEdges(graph, source, exploredSet).GetEnumerator();

        // Assert

        while (true)
        {
            bool expectedHasCurrent = enumerableSteps.MoveNext();
            bool actualHasCurrent = basicSteps.MoveNext();

            Assert.Equal(expectedHasCurrent, actualHasCurrent);

            if (!expectedHasCurrent || !actualHasCurrent)
                break;

            Int32Endpoints expected = enumerableSteps.Current;
            Int32Endpoints actual = basicSteps.Current;

            if (expected != actual)
            {
                Assert.Equal(expected, actual);
                break;
            }
        }
    }

    [Theory]
    [ClassData(typeof(UndirectedSimpleGraphCollection))]
    internal void EnumerateVertices(GraphParameter<Graph> p)
    {
        Graph graph = p.Graph;

        // Arrange

        int source = graph.VertexCount >> 1;
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet exploredSet = new(setBackingStore);

        // Act

        using IEnumerator<int> basicSteps = Dfs.EnumerateVertices(graph, source, graph.VertexCount);
        using IEnumerator<int> enumerableSteps = EnumerableDfs.EnumerateVertices(graph, source, exploredSet).GetEnumerator();

        // Assert

        while (true)
        {
            bool expectedHasCurrent = enumerableSteps.MoveNext();
            bool actualHasCurrent = basicSteps.MoveNext();

            Assert.Equal(expectedHasCurrent, actualHasCurrent);

            if (!expectedHasCurrent || !actualHasCurrent)
                break;

            int expected = enumerableSteps.Current;
            int actual = basicSteps.Current;

            if (expected != actual)
            {
                Assert.Equal(expected, actual);
                break;
            }
        }
    }
}
