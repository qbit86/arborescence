namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.IO;
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
            var jaggedAdjacencyListBuilder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexUpperBound);

            using (TextReader textReader = IndexedGraphs.GetTextReader(testName))
            {
                var parser = new IndexedEdgeListParser();
                IEnumerable<SourceTargetPair<int>> edges = parser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    jaggedAdjacencyListBuilder.TryAdd(edge.Source, edge.Target, out _);
            }

            JaggedAdjacencyListIncidenceGraph jaggedAdjacencyList = jaggedAdjacencyListBuilder.ToGraph();

            var adjacencyListBuilder = new AdjacencyListIncidenceGraphBuilder(VertexUpperBound);

            using (TextReader textReader = IndexedGraphs.GetTextReader(testName))
            {
                var parser = new IndexedEdgeListParser();
                IEnumerable<SourceTargetPair<int>> edges = parser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    adjacencyListBuilder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph adjacencyList = adjacencyListBuilder.ToGraph();

            Assert.True(true, testName);
        }
    }
}
