namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the <see cref="Create{TColorMapPolicy}"/> factory method
    /// for <see cref="EagerDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TColorMap,TColorMapPolicy}"/>.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
    public static class EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TGraph : IOutEdgesConcept<TVertex, TEdgeEnumerator>, IHeadConcept<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Creates a new <see cref="EagerDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TColorMap,TColorMapPolicy}"/>
        /// algorithm from the given policies.
        /// </summary>
        /// <param name="colorMapPolicy">
        /// The <see cref="IMapPolicy{TMap,TKey,TValue}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <typeparam name="TColorMapPolicy">The type of the vertex color map policy.</typeparam>
        /// <returns>An algorithm instance with specified policies.</returns>
        public static EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TColorMapPolicy>
            Create<TColorMapPolicy>(TColorMapPolicy colorMapPolicy)
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        {
            return new EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TColorMapPolicy>(
                colorMapPolicy);
        }
    }
}
