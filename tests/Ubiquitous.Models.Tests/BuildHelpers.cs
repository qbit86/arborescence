namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using Workbench;

    internal static class BuildHelpers<TGraph, TEdge>
    {
        internal static TGraph CreateGraph<TBuilder>(ref TBuilder builder, string testName)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            return CreateGraph(ref builder, testName, false);
        }

        internal static TGraph CreateGraph<TBuilder>(ref TBuilder builder, string testName, bool orderByTail)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            PopulateFromIndexedGraph(ref builder, testName, orderByTail);

            return builder.ToGraph();
        }

        private static void PopulateFromIndexedGraph<TBuilder>(ref TBuilder builder, string testName, bool orderByTail)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            if (string.IsNullOrWhiteSpace(testName))
                return;

            using (TextReader textReader = IndexedGraphs.GetTextReader(testName))
            {
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                if (orderByTail)
                    edges = edges.OrderBy(st => st.Tail);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }
        }
    }
}
