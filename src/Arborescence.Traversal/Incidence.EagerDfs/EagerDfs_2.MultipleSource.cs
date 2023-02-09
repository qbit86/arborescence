namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static partial class EagerDfs<TVertex, TEdge>
    {
        /// <summary>
        /// Traverses the graph in a depth-first order starting from the multiple sources
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<TVertex>
            where THandler : IDfsHandler<TVertex, TEdge, TGraph> =>
            EagerDfs<TVertex, TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, sources, handler, cancellationToken);

        /// <summary>
        /// Traverses the graph in a depth-first order starting from the multiple sources
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<TVertex>
            where THandler : IDfsHandler<TVertex, TEdge, TGraph> =>
            EagerDfs<TVertex, TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, sources, comparer, handler, cancellationToken);

        /// <summary>
        /// Traverses the graph in a depth-first order starting from the multiple sources
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="colorByVertex">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="cancellationToken">Optional token used to stop the traversal.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="colorByVertex"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TVertex, TEdge, TGraph> =>
            EagerDfs<TVertex, TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, sources, colorByVertex, handler, cancellationToken);
    }
}
