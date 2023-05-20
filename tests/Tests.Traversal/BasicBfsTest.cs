namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using Traversal.Specialized;
using Xunit;
using Graph = Models.Specialized.Int32AdjacencyGraph;
using EdgeEnumerator = IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

public sealed class BasicBfsTest
{
    private static EnumerableBfs<Graph, Endpoints<int>, EdgeEnumerator> Bfs => default;

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateEdges(GraphParameter<Graph> p)
    {
        Graph graph = p.Graph;

        // Arrange

        int source = graph.VertexCount >> 1;
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet exploredSet = new(setBackingStore);

        // Act

        using IEnumerator<Endpoints<int>> basicSteps = Bfs.EnumerateEdges(graph, source, graph.VertexCount);
        using IEnumerator<Endpoints<int>> enumerableSteps =
            Traversal.Incidence.EnumerableBfs<int, Endpoints<int>, EdgeEnumerator>.EnumerateEdges(
                graph, source, exploredSet).GetEnumerator();

        // Assert

        while (true)
        {
            bool expectedHasCurrent = enumerableSteps.MoveNext();
            bool actualHasCurrent = basicSteps.MoveNext();

            Assert.Equal(expectedHasCurrent, actualHasCurrent);

            if (!expectedHasCurrent || !actualHasCurrent)
                break;

            Endpoints<int> expected = enumerableSteps.Current;
            Endpoints<int> actual = basicSteps.Current;

            if (expected != actual)
            {
                Assert.Equal(expected, actual);
                break;
            }
        }
    }

    [Theory]
    [ClassData(typeof(Int32AdjacencyGraphCollection))]
    internal void EnumerateVertices(GraphParameter<Graph> p)
    {
        Graph graph = p.Graph;

        // Arrange

        int source = graph.VertexCount >> 1;
        byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
        Array.Clear(setBackingStore, 0, setBackingStore.Length);
        IndexedSet exploredSet = new(setBackingStore);

        // Act

        using IEnumerator<int> basicSteps = Bfs.EnumerateVertices(graph, source, graph.VertexCount);
        using IEnumerator<int> enumerableSteps = Traversal.Incidence.EnumerableBfs<int, Endpoints<int>, EdgeEnumerator>
            .EnumerateVertices(graph, source, exploredSet).GetEnumerator();

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
