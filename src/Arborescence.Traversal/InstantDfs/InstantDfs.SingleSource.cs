namespace Arborescence.Traversal
{
    using System;

    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        private static readonly Func<TGraph, TVertex, bool> s_false = (g, v) => false;

        /// <summary>
        /// Traverses the graph in a DFS manner starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="colorMap">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<THandler>(TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorMap, handler, s_false);
        }

        /// <summary>
        /// Traverses the graph in a DFS manner starting from the single source
        /// until the search tree is built or until the specified condition is satisfied.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="colorMap">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="terminationCondition">The predicate to be checked for each vertex after its discovering.</param>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<THandler>(TGraph graph, TVertex source, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorMap, handler, terminationCondition ?? s_false);
        }
    }
}
