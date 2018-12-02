namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using Workbench;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class AdjacencyListTest
    {
        private const int VertexUpperBound = 10;

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void AdjacencyList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref jaggedAdjacencyListBuilder, testName);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList = jaggedAdjacencyListBuilder.ToGraph();

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<AdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref adjacencyListBuilder, testName);
            AdjacencyListIncidenceGraph adjacencyList = adjacencyListBuilder.ToGraph();

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexUpperBound; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                var jaggedOutEdges = new List<int>(OneTimeEnumerable<int>.Create(jaggedOutEdgesEnumerator));

                bool hasOutEdges = adjacencyList.TryGetOutEdges(v, out ArraySegmentEnumerator<int> outEdgesEnumerator);
                Assert.True(hasOutEdges);

                var outEdges = new List<int>(OneTimeEnumerable<int>.Create(outEdgesEnumerator));

                IEnumerable<int> difference = jaggedOutEdges.Except(outEdges);

                // Assert
                Assert.Empty(difference);
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void AdjacencyList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref jaggedAdjacencyListBuilder, testName);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList = jaggedAdjacencyListBuilder.ToGraph();

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<AdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref adjacencyListBuilder, testName);
            AdjacencyListIncidenceGraph adjacencyList = adjacencyListBuilder.ToGraph();

            // Act
            for (int v = 0; v < adjacencyList.VertexUpperBound; ++v)
            {
                if (!adjacencyList.TryGetOutEdges(v, out ArraySegmentEnumerator<int> outEdgesEnumerator))
                    continue;

                var outEdges = new List<int>(OneTimeEnumerable<int>.Create(outEdgesEnumerator));

                bool hasOutEdges =
                    jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);
                Assert.True(hasOutEdges);

                var jaggedOutEdges = new List<int>(OneTimeEnumerable<int>.Create(jaggedOutEdgesEnumerator));

                IEnumerable<int> difference = outEdges.Except(jaggedOutEdges);

                // Assert
                Assert.Empty(difference);
            }
        }
    }
}
