namespace Arborescence.Models
{
    using System;

    public readonly partial struct SimpleIncidenceGraph
    {
#pragma warning disable CA1034 // Nested types should not be visible
        public sealed class UndirectedBuilder : IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>
        {
            private int _edgeCount;
            private ArrayPrefix<Endpoints> _edges;
            private int _vertexCount;

            public UndirectedBuilder(int initialVertexCount, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

                _edges = ArrayPrefixBuilder.Create<Endpoints>(edgeCapacity);
                _vertexCount = initialVertexCount;
            }

            public bool TryAdd(int tail, int head, out Endpoints edge)
            {
                edge = new Endpoints(tail, head);
                if (tail < 0 || head < 0)
                    return false;

                int newVertexCountCandidate = Math.Max(tail, head) + 1;
                if (newVertexCountCandidate > _vertexCount)
                    _vertexCount = newVertexCountCandidate;

                _edges = ArrayPrefixBuilder.Add(_edges, edge, false);
                ++_edgeCount;

                if (tail != head)
                {
                    var invertedEdge = new Endpoints(head, tail);
                    _edges = ArrayPrefixBuilder.Add(_edges, invertedEdge, false);
                }

                return true;
            }

            public SimpleIncidenceGraph ToGraph()
            {
                int n = _vertexCount;
                int m = _edges.Count;
                Array.Sort(_edges.Array, 0, m, EdgeComparer.Instance);

#if NET5
                Endpoints[] edgesOrderedByTail = GC.AllocateUninitializedArray<Endpoints>(m);
#else
                var edgesOrderedByTail = new Endpoints[m];
#endif
                _edges.CopyTo(edgesOrderedByTail);

                var data = new int[2 + n];
                data[0] = n;
                data[1] = m;

                Span<int> upperBoundByVertex = data.AsSpan(2);
                foreach (Endpoints edge in _edges)
                {
                    int tail = edge.Tail;
                    ++upperBoundByVertex[tail];
                }

                for (int vertex = 1; vertex < n; ++vertex)
                    upperBoundByVertex[vertex] += upperBoundByVertex[vertex - 1];

                _edges = ArrayPrefixBuilder.Release(_edges, false);
                _vertexCount = 0;

                return new SimpleIncidenceGraph(data, edgesOrderedByTail);
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
