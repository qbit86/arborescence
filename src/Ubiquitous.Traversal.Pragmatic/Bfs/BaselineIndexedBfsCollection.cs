namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphConcept>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, int, TEdge>, IGetOutEdgesConcept<TGraph, int, TEdgeEnumerator>
    {
        public BaselineIndexedBfsCollection(TGraph graph, int startVertex, int vertexCount, TGraphConcept graphConcept)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex < 0)
                throw new ArgumentOutOfRangeException(nameof(startVertex));

            if (vertexCount <= startVertex)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            Graph = graph;
            StartVertex = startVertex;
            VertexCount = vertexCount;
            GraphConcept = graphConcept;
        }

        public TGraph Graph { get; }
        public int StartVertex { get; }
        public int VertexCount { get; }
        public TGraphConcept GraphConcept { get; }

        public IEnumerator<TEdge> GetEnumerator()
        {
            if (Graph == null)
                throw new InvalidOperationException($"{nameof(Graph)}: null");

            if (StartVertex < 0)
                throw new InvalidOperationException($"{nameof(StartVertex)}: {StartVertex}");

            if (VertexCount <= StartVertex)
                throw new InvalidOperationException($"{nameof(VertexCount)}: {VertexCount}");

            if (GraphConcept == null)
                throw new InvalidOperationException($"{nameof(GraphConcept)}: null");

            return GetEnumeratorCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<TEdge> GetEnumeratorCoroutine()
        {
            var colorMap = new Color[VertexCount];
            var queue = new Queue<int>();

            Put(colorMap, StartVertex, Color.Gray);
            queue.Enqueue(StartVertex);

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                if (GraphConcept.TryGetOutEdges(Graph, u, out TEdgeEnumerator outEdges))
                {
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;

                        if (!GraphConcept.TryGetTarget(Graph, e, out int v))
                            continue;

                        Color color = Get(colorMap, v);

                        switch (color)
                        {
                            case Color.None:
                            case Color.White:
                            {
                                Put(colorMap, v, Color.Gray);
                                queue.Enqueue(v);
                                yield return e;
                                break;
                            }
                        }
                    }
                }

                Put(colorMap, u, Color.Black);
            }
        }

        private static Color Get(Color[] colorMap, int key)
        {
            Debug.Assert(colorMap != null);

            if (key < 0)
                return Color.None;

            if (colorMap.Length <= key)
                return Color.None;

            return colorMap[key];
        }

        private static void Put(Color[] colorMap, int key, Color value)
        {
            Debug.Assert(colorMap != null);

            if (key < 0)
                return;

            if (colorMap.Length <= key)
                return;

            colorMap[key] = value;
        }
    }
}
