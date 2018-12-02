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

        public AdjacencyListTest(ITestOutputHelper output)
        {
            Output = output;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private ITestOutputHelper Output { get; }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void ShouldNotBeLess(string testName)
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

                IEnumerable<int> leftDifference = jaggedOutEdges.Except(outEdges);

                // Assert
                Assert.Empty(leftDifference);
            }
        }
    }
}
