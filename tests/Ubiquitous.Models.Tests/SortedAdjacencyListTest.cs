namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Workbench;
    using Xunit;

    public sealed class SortedAdjacencyListTest
    {
        private const int VertexUpperBound = 10;

        [Theory]
        [MemberData(nameof(GetData))]
        public void SortedAdjacencyList_ShouldNotBeLess(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref jaggedAdjacencyListBuilder, testName);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList = jaggedAdjacencyListBuilder.ToGraph();

            var sortedAdjacencyListBuilder = new SortedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<SortedAdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref sortedAdjacencyListBuilder, testName);
            SortedAdjacencyListIncidenceGraph sortedAdjacencyList = sortedAdjacencyListBuilder.ToGraph();

            // Act
            for (int v = 0; v < jaggedAdjacencyList.VertexUpperBound; ++v)
            {
                if (!jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator))
                    continue;

                List<int> jaggedOutEdges = OneTimeEnumerable<int>.Create(jaggedOutEdgesEnumerator).ToList();

                bool hasOutEdges = sortedAdjacencyList.TryGetOutEdges(v, out RangeEnumerator outEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                List<int> outEdges = OneTimeEnumerable<int>.Create(outEdgesEnumerator).ToList();

                IEnumerable<int> difference = jaggedOutEdges.Except(outEdges);

                // Assert
                Assert.Empty(difference);
            }
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void SortedAdjacencyList_ShouldNotBeGreater(string testName)
        {
            // Arrange
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<JaggedAdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref jaggedAdjacencyListBuilder, testName);
            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList = jaggedAdjacencyListBuilder.ToGraph();

            var sortedAdjacencyListBuilder = new SortedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);
            BuildHelpers<SortedAdjacencyListIncidenceGraph, int>.PopulateFromIndexedGraph(
                ref sortedAdjacencyListBuilder, testName);
            SortedAdjacencyListIncidenceGraph sortedAdjacencyList = sortedAdjacencyListBuilder.ToGraph();

            // Act
            for (int v = 0; v < sortedAdjacencyList.VertexUpperBound; ++v)
            {
                if (!sortedAdjacencyList.TryGetOutEdges(v, out RangeEnumerator outEdgesEnumerator))
                    continue;

                List<int> outEdges = OneTimeEnumerable<int>.Create(outEdgesEnumerator).ToList();

                bool hasOutEdges =
                    jaggedAdjacencyList.TryGetOutEdges(v, out ArrayPrefixEnumerator<int> jaggedOutEdgesEnumerator);
                Assert.True(hasOutEdges, $"Should have edges for {nameof(v)}: {v}");

                List<int> jaggedOutEdges = OneTimeEnumerable<int>.Create(jaggedOutEdgesEnumerator).ToList();

                IEnumerable<int> difference = outEdges.Except(jaggedOutEdges);

                // Assert
                Assert.Empty(difference);
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            return new[]
            {
                new object[] { "05" },
            };
        }
    }
}
