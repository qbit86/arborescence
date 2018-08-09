namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct IndexedAdjacencyListGraph : IEquatable<IndexedAdjacencyListGraph>
    {
        private List<SourceTargetPair<int>> Endpoints { get; }
        private List<int>[] OutEdges { get; }

        internal IndexedAdjacencyListGraph(List<SourceTargetPair<int>> endpoints, List<int>[] outEdges)
        {
            Assert(endpoints != null);
            Assert(outEdges != null);

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `outEdges` are consistent. For each vertex and for each edge in outEdges(vertex): source(edge) = vertex.

            Endpoints = endpoints;
            OutEdges = outEdges;
        }

        public int VertexCount => OutEdges.Length;

        public int EdgeCount => Endpoints.Count;

        public bool TryGetEndpoints(int edge, out SourceTargetPair<int> endpoints)
        {
            if ((uint)edge >= (uint)Endpoints.Count)
            {
                endpoints = default;
                return false;
            }

            endpoints = Endpoints[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out List<int>.Enumerator outEdges)
        {
            if ((uint)vertex >= (uint)VertexCount)
            {
                outEdges = default;
                return false;
            }

            outEdges = OutEdges[vertex].GetEnumerator();
            return true;
        }

        public bool Equals(IndexedAdjacencyListGraph other)
        {
            if (!Endpoints.Equals(other.Endpoints))
                return false;

            if (!OutEdges.Equals(other.OutEdges))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is IndexedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Endpoints.GetHashCode() ^ OutEdges.GetHashCode();
        }
    }
}
