namespace Arborescence
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
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void EdgeList_ShouldNotBeLess(GraphDefinitionParameter p)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder, p);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(InitialVertexCount);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, Endpoints<int>>.CreateGraph(ref edgeListBuilder, p);

            Assert.Equal(jaggedAdjacencyList.VertexCount, edgeList.VertexCount);

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexCount; ++v)
            {
                ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator = jaggedAdjacencyList.EnumerateOutEdges(v);

                int defensiveCopy = v;
                var jaggedOutEndpoints = jaggedOutEdgesEnumerator
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetHead(e, out int head), Head = head })
                    .Where(it => it.Success)
                    .Select(it => Endpoints.Create(defensiveCopy, it.Head))
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
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void EdgeList_ShouldNotBeGreater(GraphDefinitionParameter p)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(InitialVertexCount);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList =
                BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.CreateGraph(ref jaggedAdjacencyListBuilder, p);

            var edgeListBuilder = new EdgeListIncidenceGraphBuilder(InitialVertexCount);
            EdgeListIncidenceGraph edgeList =
                BuildHelpers<EdgeListIncidenceGraph, Endpoints<int>>.CreateGraph(ref edgeListBuilder, p);

            // Act
            for (int v = 0; v < edgeList.VertexCount; ++v)
            {
                ArraySegmentEnumerator<Endpoints<int>> outEdgesEnumerator = edgeList.EnumerateOutEdges(v);

                Rist<Endpoints<int>> outEdges = RistFactory<Endpoints<int>>.Create(outEdgesEnumerator);

                ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator = jaggedAdjacencyList.EnumerateOutEdges(v);

                int defensiveCopy = v;
                var jaggedOutEndpoints = jaggedOutEdgesEnumerator
                    .Select(e => new { Success = jaggedAdjacencyList.TryGetHead(e, out int head), Head = head })
                    .Where(it => it.Success)
                    .Select(it => Endpoints.Create(defensiveCopy, it.Head))
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
