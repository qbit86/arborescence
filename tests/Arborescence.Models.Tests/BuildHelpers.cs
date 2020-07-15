namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    internal static class BuildHelpers<TGraph, TEdge>
    {
        internal static TGraph CreateGraph<TBuilder>(ref TBuilder builder, GraphDefinitionParameter parameter)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            return CreateGraph(ref builder, parameter, false);
        }

        internal static TGraph CreateGraph<TBuilder>(
            ref TBuilder builder, GraphDefinitionParameter parameter, bool orderByTail)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            PopulateFromIndexedGraph(ref builder, parameter, orderByTail);

            return builder.ToGraph();
        }

        private static void PopulateFromIndexedGraph<TBuilder>(
            ref TBuilder builder, GraphDefinitionParameter parameter, bool orderByTail)
            where TBuilder : IGraphBuilder<TGraph, int, TEdge>
        {
            if (parameter is null)
                return;

            IEnumerable<Endpoints<int>> edges = parameter.Edges;
            if (orderByTail)
                edges = edges.OrderBy(st => st.Tail);
            foreach (Endpoints<int> edge in edges)
                builder.TryAdd(edge.Tail, edge.Head, out _);
        }
    }
}
