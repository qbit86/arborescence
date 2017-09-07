namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public sealed class IndexedAdjacencyListGraphBuilder
    {
        private List<SourceTargetPair<int>> Endpoints { get; set; }
        private List<int>[] OutEdges { get; set; }

        public int VertexCount => OutEdges?.Length ?? 0;

        public IndexedAdjacencyListGraphBuilder(int vertexCount) : this(vertexCount, 0)
        {
        }

        public IndexedAdjacencyListGraphBuilder(int vertexCount, int edgeCount)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException("Non-negative number required.", nameof(vertexCount));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException("Non-negative number required.", nameof(edgeCount));

            OutEdges = new List<int>[vertexCount];
            Endpoints = edgeCount >= 0 ? new List<SourceTargetPair<int>>(edgeCount) : new List<SourceTargetPair<int>>();
        }

        public bool Add(SourceTargetPair<int> edge)
        {
            if (Endpoints == null || OutEdges == null)
                return false;

            if (edge.Source < 0 || edge.Source >= VertexCount)
                return false;

            if (edge.Target < 0 || edge.Target >= VertexCount)
                return false;

            int newEdgeIndex = Endpoints.Count;
            Endpoints.Add(edge);

            if (OutEdges[edge.Source] == null)
            {
                OutEdges[edge.Source] = new List<int> { newEdgeIndex };
            }
            else
            {
                OutEdges[edge.Source].Add(newEdgeIndex);
            }

            return true;
        }

        public IndexedAdjacencyListGraph ToIndexedAdjacencyListGraph()
        {
            var endpoints = Endpoints ?? new List<SourceTargetPair<int>>();
            var outEdges = OutEdges ?? new List<int>[0]; // Array.Empty<List<int>>() in .NET Standard 1.3.

            Endpoints = null;
            OutEdges = null;

            return new IndexedAdjacencyListGraph(endpoints, outEdges);
        }
    }
}
