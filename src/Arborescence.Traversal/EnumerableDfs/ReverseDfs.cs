namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph —
    /// where for each vertex its out-edges are enumerated in the reverse order as they are added to the LIFO-stack.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
    /// <typeparam name="TGraphPolicy">The type of the graph policy.</typeparam>
    /// <typeparam name="TExploredSetPolicy">The type of the set policy.</typeparam>
    public readonly partial struct ReverseDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        /// <summary>
        /// Creates a new
        /// <see cref="ReverseDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TExploredSet,TGraphPolicy,TExploredSetPolicy}"/>
        /// algorithm from the given policies.
        /// </summary>
        /// <param name="graphPolicy">The graph policy.</param>
        /// <param name="exploredSetPolicy">
        /// The <see cref="ISetPolicy{TSet,TElement}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graphPolicy"/> is <see langword="null"/>,
        /// or <paramref name="exploredSetPolicy"/> is <see langword="null"/>.
        /// </exception>
        public ReverseDfs(TGraphPolicy graphPolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            GraphPolicy = graphPolicy;
            ExploredSetPolicy = exploredSetPolicy;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
