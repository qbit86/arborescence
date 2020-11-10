namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the <see cref="Create{TFringePolicy,TExploredSetPolicy}"/> factory method for
    /// <see
    ///     cref="GenericSearch{TGraph,TVertex,TEdge,TEdgeEnumerator,TFringe,TExploredSet,TFringePolicy,TExploredSetPolicy}"/>
    /// .
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TFringe">The type of the generic queue.</typeparam>
    /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
    public static class GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet>
        where TGraph : IOutEdgesConcept<TVertex, TEdgeEnumerator>, IHeadConcept<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Creates a new
        /// <see
        ///     cref="GenericSearch{TGraph,TVertex,TEdge,TEdgeEnumerator,TFringe,TExploredSet,TFringePolicy,TExploredSetPolicy}"/>
        /// algorithm from the given policies.
        /// </summary>
        /// <param name="fringePolicy">
        /// The <see cref="IContainerPolicy{TContainer,TElement}"/> implementation to use as the frontier while traversing.
        /// </param>
        /// <param name="exploredSetPolicy">
        /// The <see cref="ISetPolicy{TSet,TElement}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <typeparam name="TFringePolicy">The type of the generic queue policy.</typeparam>
        /// <typeparam name="TExploredSetPolicy">The type of the set policy.</typeparam>
        /// <returns>An algorithm instance with specified policies.</returns>
        public static GenericSearch<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet, TFringePolicy, TExploredSetPolicy>
            Create<TFringePolicy, TExploredSetPolicy>(TFringePolicy fringePolicy, TExploredSetPolicy exploredSetPolicy)
            where TFringePolicy : IContainerPolicy<TFringe, TVertex>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
                TFringePolicy, TExploredSetPolicy>(
                fringePolicy, exploredSetPolicy);
        }
    }
}
