namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using Models;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<Endpoints>.Enumerator;
    using Graph = Models.SimpleIncidenceGraph;

    public sealed class BasicBfsTest
    {
        private EnumerableBfs<Graph, Endpoints, EdgeEnumerator> Bfs { get; }

        private EnumerableBfs<Graph, int, Endpoints, EdgeEnumerator> EnumerableBfs { get; }

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

            IEnumerator<Endpoints> basicSteps = Bfs.EnumerateEdges(graph, source, graph.VertexCount)!;
            IEnumerator<Endpoints> enumerableSteps = EnumerableBfs.EnumerateEdges(graph, source, exploredSet)!;

            // Assert

            while (true)
            {
                bool expectedHasCurrent = enumerableSteps.MoveNext();
                bool actualHasCurrent = basicSteps.MoveNext();

                Assert.Equal(expectedHasCurrent, actualHasCurrent);

                if (!expectedHasCurrent || !actualHasCurrent)
                    break;

                Endpoints expected = enumerableSteps.Current;
                Endpoints actual = basicSteps.Current;

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
            SimpleIncidenceGraph graph = p.Graph;

            // Arrange

            int source = graph.VertexCount >> 1;
            byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
            Array.Clear(setBackingStore, 0, setBackingStore.Length);
            IndexedSet exploredSet = new(setBackingStore);

            // Act

            IEnumerator<int> basicSteps = Bfs.EnumerateVertices(graph, source, graph.VertexCount)!;
            IEnumerator<int> enumerableSteps = EnumerableBfs.EnumerateVertices(graph, source, exploredSet)!;

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
}
