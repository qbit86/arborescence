namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Workbench;
    using Xunit;

    public sealed class EdgeListTest
    {
        private const int VertexUpperBound = 10;

        [Theory(Skip = "Not fixed yet.")]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void EdgeList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(VertexUpperBound);
            EdgeListIncidenceGraph adjacencyList =
                BuildHelpers<EdgeListIncidenceGraph, SourceTargetPair<int>>.CreateGraph(ref edgeListBuilder, testName);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexUpperBound; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                List<SourceTargetPair<int>> jaggedOutEndpoints = OneTimeEnumerable<int>
                    .Create(jaggedOutEdgesEnumerator)
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetTarget(e, out int target), Target = target })
                    .Where(p => p.Success).Select(p => SourceTargetPair.Create(v, p.Target))
                    .ToList();

                bool hasOutEdges = adjacencyList.TryGetOutEdges(v,
                    out ArraySegmentEnumerator<SourceTargetPair<int>> outEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                List<SourceTargetPair<int>> outEdges =
                    OneTimeEnumerable<SourceTargetPair<int>>.Create(outEdgesEnumerator).ToList();

                IEnumerable<SourceTargetPair<int>> difference = jaggedOutEndpoints.Except(outEdges);

                // Assert
                Assert.Empty(difference);
            }
        }
    }
}
