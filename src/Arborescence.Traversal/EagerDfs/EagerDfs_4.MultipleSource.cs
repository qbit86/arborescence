namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Traverses the graph in a DFS manner starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="colorByVertex">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="colorByVertex"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<TVertexEnumerator, TColorMap, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorByVertex, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (colorByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            while (sources.MoveNext())
            {
                TVertex u = sources.Current;
                Color color = GetColorOrDefault(colorByVertex, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorByVertex, handler, s_false);
            }
        }

        /// <summary>
        /// Traverses the graph in a DFS manner starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="colorByVertex">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="startVertex">The source to start with.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<TVertexEnumerator, TColorMap, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorByVertex, THandler handler, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorByVertex, handler, s_false);

            while (sources.MoveNext())
            {
                TVertex u = sources.Current;
                Color color = GetColorOrDefault(colorByVertex, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorByVertex, handler, s_false);
            }
        }
    }
}
