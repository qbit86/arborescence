namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Internal;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapConcept : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        public BaselineBfsCollection(TGraph graph, TVertex startVertex, int queueCapacity,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex == null)
                throw new ArgumentNullException(nameof(startVertex));

            if (queueCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(queueCapacity));

            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            Graph = graph;
            StartVertex = startVertex;
            QueueCapacity = queueCapacity;
            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private int QueueCapacity { get; }
        private TGraphConcept GraphConcept { get; }
        private TColorMapConcept ColorMapConcept { get; }

        public IEnumerator<TEdge> GetEnumerator()
        {
            if (Graph == null)
                throw new InvalidOperationException($"{nameof(Graph)}: null");

            if (StartVertex == null)
                throw new InvalidOperationException($"{nameof(StartVertex)}: null");

            if (GraphConcept == null)
                throw new InvalidOperationException($"{nameof(GraphConcept)}: null");

            if (ColorMapConcept == null)
                throw new InvalidOperationException($"{nameof(ColorMapConcept)}: null");

            TColorMap colorMap = ColorMapConcept.Acquire();
            Queue<TVertex> queue = QueueCache<TVertex>.Acquire(QueueCapacity);
            return GetEnumeratorCoroutine(colorMap, queue);
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
                if (!ColorMapConcept.TryPut(colorMap, StartVertex, Color.Gray))
                    yield break;

                queue.Enqueue(StartVertex);

                while (queue.Count > 0)
                {
                    TVertex u = queue.Dequeue();
                    if (GraphConcept.TryGetOutEdges(Graph, u, out TEdgeEnumerator outEdges))
                    {
                        while (outEdges.MoveNext())
                        {
                            TEdge e = outEdges.Current;

                            if (!GraphConcept.TryGetTarget(Graph, e, out TVertex v))
                                continue;

                            ColorMapConcept.TryGet(colorMap, v, out Color color);
                            switch (color)
                            {
                                case Color.None:
                                case Color.White:
                                {
                                    if (!ColorMapConcept.TryPut(colorMap, v, Color.Gray))
                                        continue;

                                    queue.Enqueue(v);
                                    yield return e;
                                    break;
                                }
                            }
                        }
                    }

                    ColorMapConcept.TryPut(colorMap, u, Color.Black);
                }
            }
            finally
            {
                QueueCache<TVertex>.Release(queue);
                ColorMapConcept.Release(colorMap);
            }
        }
    }
}
