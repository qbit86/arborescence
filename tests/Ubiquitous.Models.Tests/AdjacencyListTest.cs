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

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexUpperBound; ++v)
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
            for (int v = 0; v < adjacencyList.VertexUpperBound; ++v)
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
    }
}
