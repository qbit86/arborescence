namespace Arborescence.Traversal
{
    using System;
    using Internal;

    public readonly partial struct InstantBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
    {
        /// <summary>
        /// Traverses the graph in a BFS manner starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="colorMap">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IBfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<THandler>(
            TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var queue = new Queue<TVertex>();
            ColorMapPolicy.AddOrUpdate(colorMap, source, Color.Gray);
            handler.OnDiscoverVertex(graph, source);
            queue.Add(source);

            TraverseCore(graph, queue, colorMap, handler);
        }
    }
}
