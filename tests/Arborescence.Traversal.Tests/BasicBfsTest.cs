namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Models;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<Endpoints>.Enumerator;
    using Graph = Models.SimpleIncidenceGraph;
    using GraphPolicy = Models.SimpleIncidenceGraphPolicy;

    public sealed class BasicBfsTest
    {
        private Bfs<Graph, Endpoints, EdgeEnumerator> Bfs { get; }

        private EnumerableBfs<Graph, int, Endpoints, EdgeEnumerator, byte[], GraphPolicy, IndexedSetPolicy>
            EnumerableBfs { get; }

        [Theory]
        [ClassData(typeof(UndirectedSimpleGraphCollection))]
        internal void EnumerateEdges(GraphParameter<Graph> p)
        {
            SimpleIncidenceGraph graph = p.Graph;
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount >= 0, "graph.VertexCount >= 0");
            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(exploredSet, 0, exploredSet.Length);
            int source = graph.VertexCount >> 1;

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
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount >= 0, "graph.VertexCount >= 0");
            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(exploredSet, 0, exploredSet.Length);
            int source = graph.VertexCount >> 1;

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
