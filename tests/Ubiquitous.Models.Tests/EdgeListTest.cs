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
        [ClassData(typeof(TestCaseCollection))]
        public void EdgeList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(InitialVertexCount);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, Endpoints<int>>.CreateGraph(ref edgeListBuilder, testName);

            Assert.Equal(jaggedAdjacencyList.VertexCount, edgeList.VertexCount);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexCount; ++v)
            {
                ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator = jaggedAdjacencyList.EnumerateOutEdges(v);

                int defensiveCopy = v;
                var jaggedOutEndpoints = jaggedOutEdgesEnumerator
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetHead(e, out int head), Head = head })
                    .Where(p => p.Success).Select(p => Endpoints.Create(defensiveCopy, p.Head))
                    .ToRist();

                ArraySegmentEnumerator<Endpoints<int>> outEdgesEnumerator = edgeList.EnumerateOutEdges(v);

                Rist<Endpoints<int>> outEdges = RistFactory<Endpoints<int>>.Create(outEdgesEnumerator);

                IEnumerable<Endpoints<int>> difference = jaggedOutEndpoints.Except(outEdges);

                // Assert
                Assert.Empty(difference);

                jaggedOutEndpoints.Dispose();
                outEdges.Dispose();
            }
        }

        [Theory]
        [ClassData(typeof(TestCaseCollection))]
        public void EdgeList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder,
                    testName);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(InitialVertexCount);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, Endpoints<int>>.CreateGraph(ref edgeListBuilder, testName);

            // Act
            for (int v = 0; v < edgeList.VertexCount; ++v)
            {
                ArraySegmentEnumerator<Endpoints<int>> outEdgesEnumerator = edgeList.EnumerateOutEdges(v);

                Rist<Endpoints<int>> outEdges = RistFactory<Endpoints<int>>.Create(outEdgesEnumerator);

                ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator = jaggedAdjacencyList.EnumerateOutEdges(v);

                int defensiveCopy = v;
                var jaggedOutEndpoints = jaggedOutEdgesEnumerator
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetHead(e, out int head), Head = head })
                    .Where(p => p.Success).Select(p => Endpoints.Create(defensiveCopy, p.Head))
                    .ToRist();

                IEnumerable<Endpoints<int>> difference = outEdges.Except(jaggedOutEndpoints);

                // Assert
                Assert.Empty(difference);

                outEdges.Dispose();
                jaggedOutEndpoints.Dispose();
            }
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
