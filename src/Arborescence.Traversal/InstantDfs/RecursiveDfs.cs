namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a recursive fashion.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TColorMap">The type of the vertex color map.</typeparam>
    /// <typeparam name="TColorMapPolicy">The type of the vertex color map policy.</typeparam>
    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TColorMapPolicy>
        where TGraph : IOutEdgesConcept<TVertex, TEdgeEnumerator>, IHeadConcept<TVertex, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RecursiveDfs{TGraph,TVertex,TEdge,TEdgeEnumerator,TColorMap,TColorMapPolicy}"/> struct.
        /// </summary>
        /// <param name="colorMapPolicy">
        /// The <see cref="IMapPolicy{TMap,TKey,TValue}"/> implementation to use when marking explored vertices while traversing.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="colorMapPolicy"/> is <see langword="null"/>.
        /// </exception>
        public RecursiveDfs(TColorMapPolicy colorMapPolicy)
        {
            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            ColorMapPolicy = colorMapPolicy;
        }

        private TColorMapPolicy ColorMapPolicy { get; }

        private void TraverseCore<THandler>(TGraph graph, TVertex u, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(graph != null, "graph != null");
            Debug.Assert(handler != null, "handler != null");
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Gray);
            handler.OnDiscoverVertex(graph, u);

            if (terminationCondition(graph, u))
            {
                ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                handler.OnFinishVertex(graph, u);
                return;
            }

            TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
            while (outEdges.MoveNext())
            {
                TEdge e = outEdges.Current;
                if (!graph.TryGetHead(e, out TVertex v))
                    continue;

                handler.OnExamineEdge(graph, e);
                Color color = GetColorOrDefault(colorMap, v);
                if (color == Color.None || color == Color.White)
                {
                    handler.OnTreeEdge(graph, e);
                    TraverseCore(graph, v, colorMap, handler, terminationCondition);
                }
                else if (color == Color.Gray)
                    handler.OnBackEdge(graph, e);
                else
                    handler.OnForwardOrCrossEdge(graph, e);

                handler.OnFinishEdge(graph, e);
            }

            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
            handler.OnFinishVertex(graph, u);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}
