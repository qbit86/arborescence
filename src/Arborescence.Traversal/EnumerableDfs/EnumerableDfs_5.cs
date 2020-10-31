namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the <see cref="Create{TGraphPolicy,TExploredSetPolicy}"/> factory method
    /// for <see cref="EnumerableDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TExploredSet,TGraphPolicy,TExploredSetPolicy}"/>.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
    public static class EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Creates a new
        /// <see cref="EnumerableDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TExploredSet,TGraphPolicy,TExploredSetPolicy}"/>
        /// algorithm from the given policies.
        /// </summary>
        /// <param name="graphPolicy">The graph policy.</param>
        /// <param name="exploredSetPolicy">
        /// The <see cref="ISetPolicy{TSet,TElement}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <typeparam name="TGraphPolicy">The type of the graph policy.</typeparam>
        /// <typeparam name="TExploredSetPolicy">The type of the set policy.</typeparam>
        /// <returns>An algorithm instance with specified policies.</returns>
        public static EnumerableDfs<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
            Create<TGraphPolicy, TExploredSetPolicy>(TGraphPolicy graphPolicy, TExploredSetPolicy exploredSetPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new EnumerableDfs<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>(
                graphPolicy, exploredSetPolicy);
        }
    }
}
