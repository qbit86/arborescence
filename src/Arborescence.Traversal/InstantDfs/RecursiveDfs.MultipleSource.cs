namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy,
        TColorMapPolicy>
    {
        /// <summary>
        /// Traverses the graph in a DFS manner starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="colorMap">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<TVertexEnumerator, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorMap, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            while (sources.MoveNext())
            {
                TVertex u = sources.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }

        /// <summary>
        /// Traverses the graph in a DFS manner starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="colorMap">The vertex color map.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="startVertex">The source to start with.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        public void Traverse<TVertexEnumerator, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorMap, THandler handler, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);

            while (sources.MoveNext())
            {
                TVertex u = sources.Current;
                Color color = GetColorOrDefault(colorMap, u);
                if (color != Color.None && color != Color.White)
                    continue;

                handler.OnStartVertex(graph, u);
                TraverseCore(graph, u, colorMap, handler, s_false);
            }
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
