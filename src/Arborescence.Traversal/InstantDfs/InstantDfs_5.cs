namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the <see cref="Create{TGraphPolicy,TColorMapPolicy}"/> factory method
    /// for <see cref="InstantDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TColorMap,TGraphPolicy,TColorMapPolicy}"/>.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
    public static class InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        /// <summary>
        /// Creates a new <see cref="InstantDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TColorMap,TGraphPolicy,TColorMapPolicy}"/>
        /// from the given values.
        /// </summary>
        /// <param name="graphPolicy">The graph policy.</param>
        /// <param name="colorMapPolicy">
        /// The <see cref="IMapPolicy{TMap,TKey,TValue}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <typeparam name="TGraphPolicy">The type of the graph policy.</typeparam>
        /// <typeparam name="TColorMapPolicy">The type of the vertex color map policy.</typeparam>
        /// <returns>An algorithm instance with specified policies.</returns>
        public static InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        {
            return new InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
