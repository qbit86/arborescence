namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the generic search algorithm — traversal of the graph
    /// where the order of exploring discovered vertices is determined by the specified policy.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TFringe">The type of the generic queue.</typeparam>
    /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
    /// <typeparam name="TFringePolicy">The type of the generic queue policy.</typeparam>
    /// <typeparam name="TExploredSetPolicy">The type of the set policy.</typeparam>
    public readonly partial struct GenericSearch<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet, TFringePolicy, TExploredSetPolicy>
        where TGraph : IOutEdgesConcept<TVertex, TEdgeEnumerator>, IHeadConcept<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TFringePolicy : IContainerPolicy<TFringe, TVertex>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TFringePolicy FringePolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see
        ///     cref="GenericSearch{TGraph,TVertex,TEdge,TEdgeEnumerator,TFringe,TExploredSet,TFringePolicy,TExploredSetPolicy}"/>
        /// struct.
        /// </summary>
        /// <param name="fringePolicy">
        /// The <see cref="IContainerPolicy{TContainer,TElement}"/> implementation to use as the frontier while traversing.
        /// </param>
        /// <param name="exploredSetPolicy">
        /// The <see cref="ISetPolicy{TSet,TElement}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fringePolicy"/> is <see langword="null"/>,
        /// or <paramref name="exploredSetPolicy"/> is <see langword="null"/>.
        /// </exception>
        public GenericSearch(TFringePolicy fringePolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (fringePolicy == null)
                throw new ArgumentNullException(nameof(fringePolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            ExploredSetPolicy = exploredSetPolicy;
            FringePolicy = fringePolicy;
        }
    }
}
