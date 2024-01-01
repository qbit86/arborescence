namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating <see cref="AdjacencyEnumerator{TVertex, TEdge, TGraph, TEdgeEnumerator}"/> objects.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public static class AdjacencyEnumeratorFactory<TVertex, TEdge>
    {
        /// <summary>
        /// Creates a <see cref="AdjacencyEnumerator{TVertex, TEdge, TGraph, TEdgeEnumerator}"/>.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="edgeEnumerator">The enumerator for the collection of edges.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
        /// <returns>The enumerator for the heads of given edges.</returns>
        public static AdjacencyEnumerator<TVertex, TEdge, TGraph, TEdgeEnumerator> Create<TGraph, TEdgeEnumerator>(
            TGraph graph, TEdgeEnumerator edgeEnumerator)
            where TGraph : IHeadIncidence<TVertex, TEdge>
            where TEdgeEnumerator : IEnumerator<TEdge>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));
            if (edgeEnumerator is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edgeEnumerator));

            return new(graph, edgeEnumerator);
        }
    }
}
