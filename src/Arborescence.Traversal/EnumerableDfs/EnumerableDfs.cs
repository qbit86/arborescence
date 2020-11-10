namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph — implemented as enumerator.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
    /// <typeparam name="TExploredSetPolicy">The type of the set policy.</typeparam>
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
        where TGraph : IOutEdgesConcept<TVertex, TEdgeEnumerator>, IHeadConcept<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TExploredSetPolicy ExploredSetPolicy { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumerableDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TExploredSet,TExploredSetPolicy}"/>
        /// struct.
        /// </summary>
        /// <param name="exploredSetPolicy">
        /// The <see cref="ISetPolicy{TSet,TElement}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exploredSetPolicy"/> is <see langword="null"/>.
        /// </exception>
        public EnumerableDfs(TExploredSetPolicy exploredSetPolicy)
        {
            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            ExploredSetPolicy = exploredSetPolicy;
        }
    }
}
