namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
    /// <typeparam name="TColorMapPolicy">The type of the vertex color map policy.</typeparam>
    public readonly partial struct EagerBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TColorMapPolicy>
        where TGraph : IOutEdgesConcept<TVertex, TEdgeEnumerator>, IHeadConcept<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TColorMapPolicy ColorMapPolicy { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EagerBfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TColorMap,TColorMapPolicy}"/> struct.
        /// </summary>
        /// <param name="colorMapPolicy">
        /// The <see cref="IMapPolicy{TMap,TKey,TValue}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="colorMapPolicy"/> is <see langword="null"/>.
        /// </exception>
        public EagerBfs(TColorMapPolicy colorMapPolicy)
        {
            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            ColorMapPolicy = colorMapPolicy;
        }

        private void TraverseCore<THandler>(
            TGraph graph, Internal.Queue<TVertex> queue, TColorMap colorMap, THandler handler)
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(graph != null, "graph != null");
            Debug.Assert(handler != null, "handler != null");

            try
            {
                while (queue.TryTake(out TVertex u))
                {
#if DEBUG
                    Debug.Assert(GetColorOrDefault(colorMap, u) != default);
#endif
                    handler.OnExamineVertex(graph, u);
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        handler.OnExamineEdge(graph, e);
                        Color vColor = GetColorOrDefault(colorMap, v);
                        switch (vColor)
                        {
                            case Color.None:
                            case Color.White:
                                handler.OnTreeEdge(graph, e);
                                ColorMapPolicy.AddOrUpdate(colorMap, v, Color.Gray);
                                handler.OnDiscoverVertex(graph, v);
                                queue.Add(v);
                                break;
                            case Color.Gray:
                                handler.OnNonTreeGrayHeadEdge(graph, e);
                                break;
                            case Color.Black:
                                handler.OnNonTreeBlackHeadEdge(graph, e);
                                break;
                        }
                    }

                    ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                    handler.OnFinishVertex(graph, u);
                }
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}
