namespace Arborescence.Traversal.Specialized.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a recursive manner using the program stack.
    /// </summary>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public static class RecursiveDfs<TEdge>
    {
        /// <summary>
        /// Traverses the graph in a depth-first order starting from the single source
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="cancellationToken">Optional token used to stop the traversal.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where THandler : IDfsHandler<int, TEdge, TGraph> =>
            RecursiveDfs<TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, source, vertexCount, handler, cancellationToken);

        /// <summary>
        /// Traverses the graph in a depth-first order starting from the multiple sources
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="cancellationToken">Optional token used to stop the traversal.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<int>
            where THandler : IDfsHandler<int, TEdge, TGraph> =>
            RecursiveDfs<TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, sources, vertexCount, handler, cancellationToken);
    }
}
