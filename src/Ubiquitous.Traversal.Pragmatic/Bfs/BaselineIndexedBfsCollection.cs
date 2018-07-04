namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Buffers;
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphConcept>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, int, TEdge>, IGetOutEdgesConcept<TGraph, int, TEdgeEnumerator>
    {
        internal BaselineIndexedBfsCollection(TGraph graph, int startVertex, int vertexCount, TGraphConcept graphConcept)
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
            var colorMapConcept = new IndexedMapConcept<Color>(VertexCount);

            var steps = new BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArraySegment<Color>,
                TGraphConcept, IndexedMapConcept<Color>>(Graph, StartVertex, GraphConcept, colorMapConcept);
            return steps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly struct IndexedMapConcept<T>
            : IMapConcept<ArraySegment<T>, int, T>, IFactory<TGraph, ArraySegment<T>>
        {
            public IndexedMapConcept(int count)
            {
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));

                Count = count;
            }

            public int Count { get; }

            public bool TryGet(ArraySegment<T> map, int key, out T value)
            {
                if ((uint)key >= (uint)map.Count || map.Array == null)
                {
                    value = default(T);
                    return false;
                }

                value = map.Array[key];
                return true;
            }

            public bool TryPut(ArraySegment<T> map, int key, T value)
            {
                if ((uint)key >= (uint)map.Count || map.Array == null)
                    return false;

                map.Array[key] = value;
                return true;
            }

            public ArraySegment<T> Acquire(TGraph context)
            {
                T[] array = ArrayPool<T>.Shared.Rent(Count);
                return new ArraySegment<T>(array, 0, Count);
            }

            public void Release(TGraph context, ArraySegment<T> value)
            {
                ArrayPool<T>.Shared.Return(value.Array);
            }
        }
    }
}
