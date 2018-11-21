namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public readonly struct JaggedAdjacencyListGraph : IEquatable<JaggedAdjacencyListGraph>,
        IGetSource<int, int>, IGetTarget<int, int>, IGetOutEdges<int, ArrayPrefixEnumerator<int>>
    {
        private ArrayPrefix<SourceTargetPair<int>> Endpoints { get; }
        private ArrayBuilder<int>[] OutEdges { get; }

        internal JaggedAdjacencyListGraph(ArrayPrefix<SourceTargetPair<int>> endpoints, ArrayBuilder<int>[] outEdges)
        {
            Assert(endpoints.Array != null);
            Assert(outEdges != null);

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `outEdges` are consistent. For each vertex and for each edge in outEdges(vertex): source(edge) = vertex.

            Endpoints = endpoints;
            OutEdges = outEdges;
        }

        public int VertexCount => OutEdges.Length;

        public int EdgeCount => Endpoints.Count;

        public bool TryGetSource(int edge, out int source)
        {
            bool result = TryGetEndpoints(edge, out SourceTargetPair<int> endpoints);
            source = result ? endpoints.Source : default;
            return result;
        }

        public bool TryGetTarget(int edge, out int target)
        {
            bool result = TryGetEndpoints(edge, out SourceTargetPair<int> endpoints);
            target = result ? endpoints.Target : default;
            return result;
        }

        private bool TryGetEndpoints(int edge, out SourceTargetPair<int> endpoints)
        {
            if ((uint)edge >= (uint)EdgeCount)
            {
                endpoints = default;
                return false;
            }

            endpoints = Endpoints.Array[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out ArrayPrefixEnumerator<int> outEdges)
        {
            if ((uint)vertex >= (uint)VertexCount)
            {
                outEdges = default;
                return false;
            }

            if (OutEdges[vertex].Buffer == null)
            {
                outEdges = default;
                return false;
            }

            outEdges = new ArrayPrefixEnumerator<int>(OutEdges[vertex].Buffer, OutEdges[vertex].Count);
            return true;
        }

        public bool Equals(JaggedAdjacencyListGraph other)
        {
            if (!Endpoints.Equals(other.Endpoints))
                return false;

            if (!OutEdges.Equals(other.OutEdges))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is JaggedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Endpoints.GetHashCode() ^ OutEdges.GetHashCode();
        }
    }
}
