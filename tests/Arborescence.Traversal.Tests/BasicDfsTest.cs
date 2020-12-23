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

    public sealed class BasicDfsTest
    {
        private Dfs<Graph, Endpoints, EdgeEnumerator> Dfs { get; }

        private EnumerableDfs<Graph, int, Endpoints, EdgeEnumerator, byte[], IndexedSetPolicy> EnumerableDfs { get; }

        [Theory]
        [ClassData(typeof(UndirectedSimpleGraphCollection))]
        internal void EnumerateEdges(GraphParameter<Graph> p)
        {
            SimpleIncidenceGraph graph = p.Graph;
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            int source = graph.VertexCount >> 1;
            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
            Array.Clear(exploredSet, 0, exploredSet.Length);

            // Act

            IEnumerator<Endpoints> basicSteps = Dfs.EnumerateEdges(graph, source, graph.VertexCount)!;
            IEnumerator<Endpoints> enumerableSteps = EnumerableDfs.EnumerateEdges(graph, source, exploredSet)!;

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

            int source = graph.VertexCount >> 1;
            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, source + 1));
            Array.Clear(exploredSet, 0, exploredSet.Length);

            // Act

            IEnumerator<int> basicSteps = Dfs.EnumerateVertices(graph, source, graph.VertexCount)!;
            IEnumerator<int> enumerableSteps = EnumerableDfs.EnumerateVertices(graph, source, exploredSet)!;

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
