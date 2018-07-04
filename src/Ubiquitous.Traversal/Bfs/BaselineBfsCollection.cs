namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Internal;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, TVertex, TEdge>,
        IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
    {
        public BaselineBfsCollection(TGraph graph, TVertex startVertex,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex == null)
                throw new ArgumentNullException(nameof(startVertex));

            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            Graph = graph;
            StartVertex = startVertex;
            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public TGraph Graph { get; }
        public TVertex StartVertex { get; }
        public TGraphConcept GraphConcept { get; }
        public TColorMapConcept ColorMapConcept { get; }

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

            return GetEnumeratorCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<TEdge> GetEnumeratorCoroutine()
        {
            TColorMap colorMap = ColorMapConcept.Acquire(Graph);
            Queue<TVertex> queue = QueuePool<TVertex>.Shared.Rent();
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
                QueuePool<TVertex>.Shared.Return(queue);
                ColorMapConcept.Release(Graph, colorMap);
            }
        }
    }
}
