namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the <see cref="Create{TGraphPolicy,TFringePolicy,TExploredSetPolicy}"/> factory method for
    /// <see
    ///     cref="GenericSearch{TGraph,TVertex,TEdge,TEdgeEnumerator,TFringe,TExploredSet,TGraphPolicy,TFringePolicy,TExploredSetPolicy}"/>
    /// .
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TFringe">The type of the generic queue.</typeparam>
    /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
    public static class GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        /// <summary>
        /// Creates a new
        /// <see
        ///     cref="GenericSearch{TGraph,TVertex,TEdge,TEdgeEnumerator,TFringe,TExploredSet,TGraphPolicy,TFringePolicy,TExploredSetPolicy}"/>
        /// algorithm from the given policies.
        /// </summary>
        /// <param name="graphPolicy">The graph policy.</param>
        /// <param name="fringePolicy">
        /// The <see cref="IContainerPolicy{TContainer,TElement}"/> implementation to use as the frontier while traversing.
        /// </param>
        /// <param name="exploredSetPolicy">
        /// The <see cref="ISetPolicy{TSet,TElement}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <typeparam name="TGraphPolicy">The type of the graph policy.</typeparam>
        /// <typeparam name="TFringePolicy">The type of the generic queue policy.</typeparam>
        /// <typeparam name="TExploredSetPolicy">The type of the set policy.</typeparam>
        /// <returns>An algorithm instance with specified policies.</returns>
        public static GenericSearch<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
                TGraphPolicy, TFringePolicy, TExploredSetPolicy>
            Create<TGraphPolicy, TFringePolicy, TExploredSetPolicy>(
                TGraphPolicy graphPolicy, TFringePolicy fringePolicy, TExploredSetPolicy exploredSetPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TFringePolicy : IContainerPolicy<TFringe, TVertex>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
                TGraphPolicy, TFringePolicy, TExploredSetPolicy>(
                graphPolicy, fringePolicy, exploredSetPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
