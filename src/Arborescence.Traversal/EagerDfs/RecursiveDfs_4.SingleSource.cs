namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Traverses the graph in a DFS manner starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="colorByVertex">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="colorByVertex"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (colorByVertex == null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (handler == null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorByVertex, handler, s_false);
        }

        /// <summary>
        /// Traverses the graph in a DFS manner starting from the single source
        /// until the search tree is built or until the specified condition is satisfied.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="colorByVertex">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="terminationCondition">The predicate to be checked for each vertex after its discovering.</param>
        /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (handler == null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorByVertex, handler, terminationCondition ?? s_false);
        }
    }
}
