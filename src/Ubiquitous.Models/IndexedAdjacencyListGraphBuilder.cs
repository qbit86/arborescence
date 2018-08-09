namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public sealed class IndexedAdjacencyListGraphBuilder
    {
        public IndexedAdjacencyListGraphBuilder(int vertexCount) : this(vertexCount, 0)
        {
        }

        public IndexedAdjacencyListGraphBuilder(int vertexCount, int edgeCount)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            OutEdges = new List<int>[vertexCount];
            Endpoints = edgeCount > 0 ? new List<SourceTargetPair<int>>(edgeCount) : new List<SourceTargetPair<int>>();
        }

        private List<SourceTargetPair<int>> Endpoints { get; set; }
        private List<int>[] OutEdges { get; set; }

        public int VertexCount => OutEdges?.Length ?? 0;

        public bool Add(SourceTargetPair<int> edge)
        {
            if (Endpoints == null || OutEdges == null)
                return false;

            if ((uint)edge.Source >= (uint)VertexCount)
                return false;

            if ((uint)edge.Target >= (uint)VertexCount)
                return false;

            int newEdgeIndex = Endpoints.Count;
            Endpoints.Add(edge);

            if (OutEdges[edge.Source] == null)
                OutEdges[edge.Source] = new List<int>(1);

            OutEdges[edge.Source].Add(newEdgeIndex);

            return true;
        }

        public IndexedAdjacencyListGraph MoveToIndexedAdjacencyListGraph()
        {
            List<SourceTargetPair<int>> endpoints = Endpoints ?? new List<SourceTargetPair<int>>(0);
            List<int>[] outEdges = OutEdges ?? new List<int>[0];

            Endpoints = null;
            OutEdges = null;

            return new IndexedAdjacencyListGraph(endpoints, outEdges);
        }
    }
}
