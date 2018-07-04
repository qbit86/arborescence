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
            var colorMapConcept = new IndexedColorMapConcept(VertexCount);

            var steps = new BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArraySegment<Color>,
                TGraphConcept, IndexedColorMapConcept>(Graph, StartVertex, GraphConcept, colorMapConcept);
            return steps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // TODO: Refactor via IndexedMapConcept<T>.
        private readonly struct IndexedColorMapConcept
            : IMapConcept<ArraySegment<Color>, int, Color>, IFactory<TGraph, ArraySegment<Color>>
        {
            public IndexedColorMapConcept(int vertexCount)
            {
                if (vertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(vertexCount));

                VertexCount = vertexCount;
            }

            public int VertexCount { get; }

            public bool TryGet(ArraySegment<Color> map, int key, out Color value)
            {
                if ((uint)key >= (uint)map.Count || map.Array == null)
                {
                    value = default(Color);
                    return false;
                }

                value = map.Array[key];
                return true;
            }

            public bool TryPut(ArraySegment<Color> map, int key, Color value)
            {
                if ((uint)key >= (uint)map.Count || map.Array == null)
                    return false;

                map.Array[key] = value;
                return true;
            }

            public ArraySegment<Color> Acquire(TGraph context)
            {
                Color[] array = ArrayPool<Color>.Shared.Rent(VertexCount);
                return new ArraySegment<Color>(array, 0, VertexCount);
            }

            public void Release(TGraph context, ArraySegment<Color> value)
            {
                ArrayPool<Color>.Shared.Return(value.Array);
            }
        }
    }
}
