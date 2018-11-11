namespace Ubiquitous
{
    using System;

    public sealed class IndexedAdjacencyListGraphBuilder
    {
        private ArrayBuilder<SourceTargetPair<int>> _endpoints;

        public IndexedAdjacencyListGraphBuilder(int vertexCount) : this(vertexCount, 0)
        {
        }

        public IndexedAdjacencyListGraphBuilder(int vertexCount, int edgeCount)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            OutEdges = new ArrayBuilder<int>[vertexCount];
            _endpoints = new ArrayBuilder<SourceTargetPair<int>>(edgeCount);
        }

        private ArrayBuilder<int>[] OutEdges { get; set; }

        public int VertexCount => OutEdges?.Length ?? 0;

        public int Add(SourceTargetPair<int> edge)
        {
            if (OutEdges == null)
                return -1;

            if ((uint)edge.Source >= (uint)VertexCount)
                return -1;

            if ((uint)edge.Target >= (uint)VertexCount)
                return -1;

            int newEdgeIndex = _endpoints.Count;
            _endpoints.Add(edge);

            if (OutEdges[edge.Source].Buffer == null)
                OutEdges[edge.Source] = new ArrayBuilder<int>(1);

            OutEdges[edge.Source].Add(newEdgeIndex);

            return OutEdges[edge.Source].Count - 1;
        }

        public IndexedAdjacencyListGraph MoveToIndexedAdjacencyListGraph()
        {
            ArrayPrefix<SourceTargetPair<int>> endpoints = _endpoints.Count > 0
                ? new ArrayPrefix<SourceTargetPair<int>>(_endpoints.Buffer, _endpoints.Count)
                : ArrayPrefix<SourceTargetPair<int>>.Empty;
            _endpoints = default;

            ArrayBuilder<int>[] outEdges = OutEdges ?? new ArrayBuilder<int>[0];
            OutEdges = null;

            return new IndexedAdjacencyListGraph(endpoints, outEdges);
        }
    }
}
