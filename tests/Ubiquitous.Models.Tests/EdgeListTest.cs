namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Misnomer.Extensions;
    using Models;
    using Xunit;

    public sealed class EdgeListTest
    {
        private const int InitialVertexCount = 0;

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void EdgeList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(InitialVertexCount);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, SourceTargetPair<int>>.CreateGraph(ref edgeListBuilder, testName);

            Assert.Equal(jaggedAdjacencyList.VertexCount, edgeList.VertexCount);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexCount; ++v)
            {
                jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);

                int defensiveCopy = v;
                Rist<SourceTargetPair<int>> jaggedOutEndpoints = jaggedOutEdgesEnumerator
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetTarget(e, out int target), Target = target })
                    .Where(p => p.Success).Select(p => SourceTargetPair.Create(defensiveCopy, p.Target))
                    .ToRist();

                edgeList.TryGetOutEdges(v, out ArraySegmentEnumerator<SourceTargetPair<int>> outEdgesEnumerator);

                Rist<SourceTargetPair<int>> outEdges = RistFactory<SourceTargetPair<int>>.Create(outEdgesEnumerator);

                IEnumerable<SourceTargetPair<int>> difference = jaggedOutEndpoints.Except(outEdges);

                // Assert
                Assert.Empty(difference);

                jaggedOutEndpoints.Dispose();
                outEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(IndexedGraphTestCollection))]
        public void EdgeList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(InitialVertexCount);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, SourceTargetPair<int>>.CreateGraph(ref edgeListBuilder, testName);

            // Act
            for (int v = 0; v < edgeList.VertexCount; ++v)
            {
                edgeList.TryGetOutEdges(v, out ArraySegmentEnumerator<SourceTargetPair<int>> outEdgesEnumerator);

                Rist<SourceTargetPair<int>> outEdges = RistFactory<SourceTargetPair<int>>.Create(outEdgesEnumerator);

                jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);

                int defensiveCopy = v;
                Rist<SourceTargetPair<int>> jaggedOutEndpoints = jaggedOutEdgesEnumerator
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetTarget(e, out int target), Target = target })
                    .Where(p => p.Success).Select(p => SourceTargetPair.Create(defensiveCopy, p.Target))
                    .ToRist();

                IEnumerable<SourceTargetPair<int>> difference = outEdges.Except(jaggedOutEndpoints);

                // Assert
                Assert.Empty(difference);

                outEdges.Dispose();
                jaggedOutEndpoints.Dispose();
            }
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
