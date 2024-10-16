namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating <see cref="AdjacencyEnumerator{TVertex, TEdge, TGraph, TEdgeEnumerator}"/> objects.
    /// </summary>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static class AdjacencyEnumerator<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Creates a <see cref="AdjacencyEnumerator{TVertex, TEdge, TGraph, TEdgeEnumerator}"/>.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="vertex">The vertex whose out-edges are to be enumerated.</param>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>The enumerator for the heads of given edges.</returns>
        public static AdjacencyEnumerator<TVertex, TEdge, TGraph, TEdgeEnumerator> Create<TVertex, TGraph>(
            TGraph graph, TVertex vertex)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            var edgeEnumerator = graph.EnumerateOutEdges(vertex);
            return new(graph, edgeEnumerator);
        }
    }
}
