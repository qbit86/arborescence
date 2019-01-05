namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Models;
    using Xunit;

    public sealed class AdjacencyListTest
    {
        private const int InitialVertexUpperBound = 4;

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void AdjacencyList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(InitialVertexUpperBound);
            AdjacencyListIncidenceGraph adjacencyList = BuildHelpers<AdjacencyListIncidenceGraph, int>.CreateGraph(
                ref adjacencyListBuilder, testName);

            Assert.Equal(jaggedAdjacencyList.VertexCount, adjacencyList.VertexCount);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexCount; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                Rist<int> jaggedOutEdges = RistFactory<int>.Create(jaggedOutEdgesEnumerator);

                bool hasOutEdges = adjacencyList.TryGetOutEdges(v, out ArraySegmentEnumerator<int> outEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<int> outEdges = RistFactory<int>.Create(outEdgesEnumerator);

                IEnumerable<int> difference = jaggedOutEdges.Except(outEdges);

                // Assert
                Assert.Empty(difference);

                jaggedOutEdges.Dispose();
                outEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void AdjacencyList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(InitialVertexUpperBound);
            AdjacencyListIncidenceGraph adjacencyList =
                BuildHelpers<AdjacencyListIncidenceGraph, int>.CreateGraph(ref adjacencyListBuilder, testName);

            // Act
            for (int v = 0; v < adjacencyList.VertexCount; ++v)
            {
                if (!adjacencyList.TryGetOutEdges(v, out ArraySegmentEnumerator<int> outEdgesEnumerator))
                    continue;

                Rist<int> outEdges = RistFactory<int>.Create(outEdgesEnumerator);

                bool hasOutEdges =
                    jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<int> jaggedOutEdges = RistFactory<int>.Create(jaggedOutEdgesEnumerator);

                IEnumerable<int> difference = outEdges.Except(jaggedOutEdges);

                // Assert
                Assert.Empty(difference);

                outEdges.Dispose();
                jaggedOutEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void AdjacencyList_ShouldHaveSameEndpoints(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(InitialVertexUpperBound);
            AdjacencyListIncidenceGraph adjacencyList =
                BuildHelpers<AdjacencyListIncidenceGraph, int>.CreateGraph(ref adjacencyListBuilder, testName);

            int actualEdgeCount = adjacencyList.EdgeCount;
            Assert.Equal(jaggedAdjacencyList.EdgeCount, actualEdgeCount);

            // Act
            for (int e = 0; e < actualEdgeCount; ++e)
            {
                bool hasExpectedSource = jaggedAdjacencyList.TryGetSource(e, out int expectedSource);
                bool hasActualSource = adjacencyList.TryGetSource(e, out int actualSource);

                Assert.Equal(hasExpectedSource, hasActualSource);
                Assert.Equal(expectedSource, actualSource);

                bool hasExpectedTarget = jaggedAdjacencyList.TryGetTarget(e, out int expectedTarget);
                bool hasActualTarget = adjacencyList.TryGetTarget(e, out int actualTarget);

                Assert.Equal(hasExpectedTarget, hasActualTarget);
                Assert.Equal(expectedTarget, actualTarget);
            }
        }
    }
}
