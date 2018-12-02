namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using Workbench;

    internal static class BuildHelpers<TGraph, TEdge>
    {
        internal static void PopulateFromIndexedGraph<TBuilder>(ref TBuilder builder, string testName)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            PopulateFromIndexedGraph(ref builder, testName, false);
        }

        internal static void PopulateFromIndexedGraph<TBuilder>(ref TBuilder builder, string testName,
            bool orderBySource)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            if (string.IsNullOrWhiteSpace(testName))
                return;

            using (TextReader textReader = IndexedGraphs.GetTextReader(testName))
            {
                var parser = new IndexedEdgeListParser();
                IEnumerable<SourceTargetPair<int>> edges = parser.ParseEdges(textReader);
                if (orderBySource)
                    edges = edges.OrderBy(st => st.Source);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }
        }
    }
}
