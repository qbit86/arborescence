namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Misnomer.Extensions;
    using Models;
    using Workbench;
    using Xunit;

    public sealed class AdjacencyListTest
    {
        private const int VertexUpperBound = 10;

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void AdjacencyList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            AdjacencyListIncidenceGraph adjacencyList = BuildHelpers<AdjacencyListIncidenceGraph, int>.CreateGraph(
                ref adjacencyListBuilder, testName);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexUpperBound; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                Rist<int> jaggedOutEdges = OneTimeEnumerable<int>.Create(jaggedOutEdgesEnumerator).ToRist();

                bool hasOutEdges = adjacencyList.TryGetOutEdges(v, out ArraySegmentEnumerator<int> outEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<int> outEdges = OneTimeEnumerable<int>.Create(outEdgesEnumerator).ToRist();

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
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            AdjacencyListIncidenceGraph adjacencyList =
                BuildHelpers<AdjacencyListIncidenceGraph, int>.CreateGraph(ref adjacencyListBuilder, testName);

            // Act
            for (int v = 0; v < adjacencyList.VertexUpperBound; ++v)
            {
                if (!adjacencyList.TryGetOutEdges(v, out ArraySegmentEnumerator<int> outEdgesEnumerator))
                    continue;

                Rist<int> outEdges = OneTimeEnumerable<int>.Create(outEdgesEnumerator).ToRist();

                bool hasOutEdges =
                    jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<int> jaggedOutEdges = OneTimeEnumerable<int>.Create(jaggedOutEdgesEnumerator).ToRist();

                IEnumerable<int> difference = outEdges.Except(jaggedOutEdges);

                // Assert
                Assert.Empty(difference);

                outEdges.Dispose();
                jaggedOutEdges.Dispose();
            }
        }
    }
}
