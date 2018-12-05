namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Misnomer.Extensions;
    using Models;
    using Workbench;
    using Xunit;

    public sealed class EdgeListTest
    {
        private const int VertexUpperBound = 10;

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void EdgeList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(VertexUpperBound);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, SourceTargetPair<int>>.CreateGraph(ref edgeListBuilder, testName);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexUpperBound; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                int defensiveCopy = v;
                Rist<SourceTargetPair<int>> jaggedOutEndpoints = OneTimeEnumerable<int>
                    .Create(jaggedOutEdgesEnumerator)
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetTarget(e, out int target), Target = target })
                    .Where(p => p.Success).Select(p => SourceTargetPair.Create(defensiveCopy, p.Target))
                    .ToRist();

                bool hasOutEdges = edgeList.TryGetOutEdges(v,
                    out ArraySegmentEnumerator<SourceTargetPair<int>> outEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                Rist<SourceTargetPair<int>> outEdges =
                    OneTimeEnumerable<SourceTargetPair<int>>.Create(outEdgesEnumerator).ToRist();

                IEnumerable<SourceTargetPair<int>> difference = jaggedOutEndpoints.Except(outEdges);

                // Assert
                Assert.Empty(difference);
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void EdgeList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(VertexUpperBound);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, SourceTargetPair<int>>.CreateGraph(ref edgeListBuilder, testName);

            // Act
            for (int v = 0; v < edgeList.VertexUpperBound; ++v)
            {
                if (!edgeList.TryGetOutEdges(v, out ArraySegmentEnumerator<SourceTargetPair<int>> outEdgesEnumerator))
                    continue;

                Rist<SourceTargetPair<int>> outEdges =
                    OneTimeEnumerable<SourceTargetPair<int>>.Create(outEdgesEnumerator).ToRist();

                bool hasOutEdges =
                    jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                int defensiveCopy = v;
                Rist<SourceTargetPair<int>> jaggedOutEndpoints = OneTimeEnumerable<int>
                    .Create(jaggedOutEdgesEnumerator)
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetTarget(e, out int target), Target = target })
                    .Where(p => p.Success).Select(p => SourceTargetPair.Create(defensiveCopy, p.Target))
                    .ToRist();

                IEnumerable<SourceTargetPair<int>> difference = outEdges.Except(jaggedOutEndpoints);

                // Assert
                Assert.Empty(difference);
            }
        }
    }
}
