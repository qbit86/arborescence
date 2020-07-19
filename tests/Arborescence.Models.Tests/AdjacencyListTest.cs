namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Models;
    using Xunit;

    public sealed class AdjacencyListTest
    {
        private const int InitialVertexCount = 0;

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void AdjacencyList_ShouldNotBeLess(GraphDefinitionParameter p)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder, p);

            IndexedIncidenceGraph.Builder adjacencyListBuilder = new IndexedIncidenceGraph.Builder(InitialVertexCount);
            IndexedIncidenceGraph adjacencyList = BuildHelpers<IndexedIncidenceGraph, int>.CreateGraph(
                ref adjacencyListBuilder, p);

            Assert.Equal(jaggedAdjacencyList.VertexCount, adjacencyList.VertexCount);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexCount; ++v)
            {
                ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator = jaggedAdjacencyList.EnumerateOutEdges(v);

                Rist<int> jaggedOutEdges = RistFactory<int>.Create(jaggedOutEdgesEnumerator);

                ArraySegmentEnumerator<int> outEdgesEnumerator = adjacencyList.EnumerateOutEdges(v);

                Rist<int> outEdges = RistFactory<int>.Create(outEdgesEnumerator);

                IEnumerable<int> difference = jaggedOutEdges.Except(outEdges);

                // Assert
                Assert.Empty(difference);

                jaggedOutEdges.Dispose();
                outEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void AdjacencyList_ShouldNotBeGreater(GraphDefinitionParameter p)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder, p);

            IndexedIncidenceGraph.Builder adjacencyListBuilder = new IndexedIncidenceGraph.Builder(InitialVertexCount);
            IndexedIncidenceGraph adjacencyList =
                BuildHelpers<IndexedIncidenceGraph, int>.CreateGraph(ref adjacencyListBuilder, p);

            // Act
            for (int v = 0; v < adjacencyList.VertexCount; ++v)
            {
                ArraySegmentEnumerator<int> outEdgesEnumerator = adjacencyList.EnumerateOutEdges(v);

                Rist<int> outEdges = RistFactory<int>.Create(outEdgesEnumerator);

                ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator = jaggedAdjacencyList.EnumerateOutEdges(v);

                Rist<int> jaggedOutEdges = RistFactory<int>.Create(jaggedOutEdgesEnumerator);

                IEnumerable<int> difference = outEdges.Except(jaggedOutEdges);

                // Assert
                Assert.Empty(difference);

                outEdges.Dispose();
                jaggedOutEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void AdjacencyList_ShouldHaveSameEndpoints(GraphDefinitionParameter p)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder, p);

            IndexedIncidenceGraph.Builder adjacencyListBuilder = new IndexedIncidenceGraph.Builder(InitialVertexCount);
            IndexedIncidenceGraph adjacencyList =
                BuildHelpers<IndexedIncidenceGraph, int>.CreateGraph(ref adjacencyListBuilder, p);

            int actualEdgeCount = adjacencyList.EdgeCount;
            Assert.Equal(jaggedAdjacencyList.EdgeCount, actualEdgeCount);

            // Act
            for (int e = 0; e < actualEdgeCount; ++e)
            {
                bool hasExpectedTail = jaggedAdjacencyList.TryGetTail(e, out int expectedTail);
                bool hasActualTail = adjacencyList.TryGetTail(e, out int actualTail);

                Assert.Equal(hasExpectedTail, hasActualTail);
                Assert.Equal(expectedTail, actualTail);

                bool hasExpectedHead = jaggedAdjacencyList.TryGetHead(e, out int expectedHead);
                bool hasActualHead = adjacencyList.TryGetHead(e, out int actualHead);

                Assert.Equal(hasExpectedHead, hasActualHead);
                Assert.Equal(expectedHead, actualHead);
            }
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
