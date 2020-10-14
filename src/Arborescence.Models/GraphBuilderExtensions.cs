namespace Arborescence.Models
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="IGraphBuilder{TGraph,TVertex,TEdge}"/>.
    /// </summary>
    public static class GraphBuilderExtensions
    {
        /// <summary>
        /// Attempts to add the edge with the specified endpoints to the graph.
        /// </summary>
        /// <param name="graphBuilder">The graph builder to add the edge to.</param>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        /// <returns>A value indicating whether the edge was added successfully.</returns>
        public static bool TryAdd<TGraph, TVertex, TEdge>(
            this IGraphBuilder<TGraph, TVertex, TEdge> graphBuilder, TVertex tail, TVertex head)
        {
            if (graphBuilder is null)
                throw new ArgumentNullException(nameof(graphBuilder));

            return graphBuilder.TryAdd(tail, head, out TEdge _);
        }
    }
}
