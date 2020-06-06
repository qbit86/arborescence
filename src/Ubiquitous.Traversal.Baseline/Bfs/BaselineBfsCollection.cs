namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Internal;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public BaselineBfsCollection(TGraph graph, TVertex startVertex, TColorMap colorMap, int queueCapacity,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex == null)
                throw new ArgumentNullException(nameof(startVertex));

            if (queueCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(queueCapacity));

            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            Graph = graph;
            StartVertex = startVertex;
            ColorMap = colorMap;
            QueueCapacity = queueCapacity;
            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private TColorMap ColorMap { get; }
        private int QueueCapacity { get; }
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

        public IEnumerator<TEdge> GetEnumerator()
        {
#pragma warning disable CA1303
            if (Graph == null)
                throw new InvalidOperationException($"{nameof(Graph)}: null");

            if (StartVertex == null)
                throw new InvalidOperationException($"{nameof(StartVertex)}: null");

            if (GraphPolicy == null)
                throw new InvalidOperationException($"{nameof(GraphPolicy)}: null");

            if (ColorMapPolicy == null)
                throw new InvalidOperationException($"{nameof(ColorMapPolicy)}: null");
#pragma warning restore CA1303

            Queue<TVertex> queue = QueueCache<TVertex>.Acquire(QueueCapacity);
            return GetEnumeratorCoroutine(ColorMap, queue);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<TEdge> GetEnumeratorCoroutine(TColorMap colorMap, Queue<TVertex> queue)
        {
            Debug.Assert(colorMap != null);
            Debug.Assert(queue != null);

            try
            {
                ColorMapPolicy.AddOrUpdate(colorMap, StartVertex, Color.Gray);
                queue.Enqueue(StartVertex);

                while (queue.Count > 0)
                {
                    TVertex u = queue.Dequeue();
                    TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(Graph, u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;

                        if (!GraphPolicy.TryGetHead(Graph, e, out TVertex v))
                            continue;

                        Color color = GetColorOrDefault(colorMap, v);
                        if (color == Color.None || color == Color.White)
                        {
                            ColorMapPolicy.AddOrUpdate(colorMap, v, Color.Gray);
                            queue.Enqueue(v);
                            yield return e;
                        }
                    }

                    ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                }
            }
            finally
            {
                QueueCache<TVertex>.Release(queue);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}
