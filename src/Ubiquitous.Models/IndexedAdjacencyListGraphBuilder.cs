namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    public sealed class IndexedAdjacencyListGraphBuilder
    {
        private List<SourceTargetPair<int>> Endpoints { get; set; }
        private ImmutableArray<int>.Builder[] OutEdges { get; set; }

        public int VertexCount => OutEdges?.Length ?? 0;

        public IndexedAdjacencyListGraphBuilder(int vertexCount) : this(vertexCount, 0)
        {
        }

        public IndexedAdjacencyListGraphBuilder(int vertexCount, int edgeCount)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            OutEdges = new ImmutableArray<int>.Builder[vertexCount];
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
                OutEdges[edge.Source] = ImmutableArray.CreateBuilder<int>(1);
            }

            OutEdges[edge.Source].Add(newEdgeIndex);

            return true;
        }

        public IndexedAdjacencyListGraph MoveToIndexedAdjacencyListGraph()
        {
            var endpoints = Endpoints ?? new List<SourceTargetPair<int>>(0);
            ImmutableArray<int>[] outEdges = OutEdges?.Select(CreateImmutableArray).ToArray()
                ?? new ImmutableArray<int>[0]; // Array.Empty<ImmutableArray<int>>() in .NET Standard 1.3.

            Endpoints = null;
            OutEdges = null;

            return new IndexedAdjacencyListGraph(endpoints, outEdges);
        }

        private ImmutableArray<int> CreateImmutableArray(ImmutableArray<int>.Builder builder)
        {
            if (builder == null)
                return ImmutableArray<int>.Empty;

            if (builder.Count == builder.Capacity)
                return builder.MoveToImmutable();

            return builder.ToImmutable();
        }
    }
}
