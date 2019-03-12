namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Models;
    using Xunit;

    public sealed class SortedAdjacencyListTest
    {
        private const int InitialVertexCount = 0;

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void SortedAdjacencyList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName, true);

            var sortedAdjacencyListBuilder = new SortedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            SortedAdjacencyListIncidenceGraph sortedAdjacencyList =
                BuildHelpers<SortedAdjacencyListIncidenceGraph, int>.CreateGraph(ref sortedAdjacencyListBuilder,
                    testName, true);

            Assert.Equal(jaggedAdjacencyList.VertexCount, sortedAdjacencyList.VertexCount);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexCount; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                Rist<int> jaggedOutEdges = RistFactory<int>.Create(jaggedOutEdgesEnumerator);

                bool hasOutEdges = sortedAdjacencyList.TryGetOutEdges(v, out RangeEnumerator outEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<int> outEdges = RistFactory<int>.Create(outEdgesEnumerator);

                IEnumerable<int> difference = jaggedOutEdges.Except(outEdges);

                // Assert
                Assert.Empty(difference);

                outEdges.Dispose();
                jaggedOutEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void SortedAdjacencyList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName, true);

            var sortedAdjacencyListBuilder = new SortedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            SortedAdjacencyListIncidenceGraph sortedAdjacencyList =
                BuildHelpers<SortedAdjacencyListIncidenceGraph, int>.CreateGraph(ref sortedAdjacencyListBuilder,
                    testName, true);

            Assert.Equal(jaggedAdjacencyList.VertexCount, sortedAdjacencyList.VertexCount);

            // Act
            for (int v = 0; v < sortedAdjacencyList.VertexCount; ++v)
            {
                if (!sortedAdjacencyList.TryGetOutEdges(v, out RangeEnumerator outEdgesEnumerator))
                    continue;

                Rist<int> outEdges = RistFactory<int>.Create(outEdgesEnumerator);

                bool hasOutEdges =
                    jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<int> jaggedOutEdges = RistFactory<int>.Create(jaggedOutEdgesEnumerator);

                IEnumerable<int> difference = outEdges.Except(jaggedOutEdges);

                // Assert
                Assert.Empty(difference);

                jaggedOutEdges.Dispose();
                outEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void SortedAdjacencyList_ShouldHaveSameEndpoints(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName, true);

            var sortedAdjacencyListBuilder = new SortedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            SortedAdjacencyListIncidenceGraph sortedAdjacencyList =
                BuildHelpers<SortedAdjacencyListIncidenceGraph, int>.CreateGraph(ref sortedAdjacencyListBuilder,
                    testName, true);

            int actualEdgeCount = sortedAdjacencyList.EdgeCount;
            Assert.Equal(jaggedAdjacencyList.EdgeCount, actualEdgeCount);

            // Act
            for (int e = 0; e < actualEdgeCount; ++e)
            {
                bool hasExpectedSource = jaggedAdjacencyList.TryGetSource(e, out int expectedSource);
                bool hasActualSource = sortedAdjacencyList.TryGetSource(e, out int actualSource);

                Assert.Equal(hasExpectedSource, hasActualSource);
                Assert.Equal(expectedSource, actualSource);

                bool hasExpectedTarget = jaggedAdjacencyList.TryGetTarget(e, out int expectedTarget);
                bool hasActualTarget = sortedAdjacencyList.TryGetTarget(e, out int actualTarget);

                Assert.Equal(hasExpectedTarget, hasActualTarget);
                Assert.Equal(expectedTarget, actualTarget);
            }
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
