namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static partial class EagerBfs<TVertex>
    {
        /// <summary>
        /// Traverses the graph in a breadth-first order starting from the single source
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="handler">
        /// The <see cref="IBfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            EagerBfs<TVertex, IEnumerator<TVertex>>.TraverseChecked(graph, source, handler, cancellationToken);

        /// <summary>
        /// Traverses the graph in a breadth-first order starting from the single source
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <param name="handler">
        /// The <see cref="IBfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            EagerBfs<TVertex, IEnumerator<TVertex>>.TraverseChecked(
                graph, source, comparer, handler, cancellationToken);

        /// <summary>
        /// Traverses the graph in a breadth-first order starting from the single source
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="colorByVertex">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IBfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="cancellationToken">Optional token used to stop the traversal.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="colorByVertex"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            EagerBfs<TVertex, IEnumerator<TVertex>>.TraverseChecked(
                graph, source, colorByVertex, handler, cancellationToken);
    }
}
