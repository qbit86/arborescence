namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public readonly struct JaggedAdjacencyListGraph : IEquatable<JaggedAdjacencyListGraph>,
        IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>
    {
        // Layout: endpoints start with targets, then sources follow.
        internal JaggedAdjacencyListGraph(int[] endpoints, ArrayBuilder<int>[] outEdges)
        {
            Assert(endpoints != null, "endpoints != null");
            Assert(outEdges != null, "outEdges != null");

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `outEdges` are consistent. For each vertex and for each edge in outEdges(vertex): source(edge) = vertex.

            Endpoints = endpoints;
            OutEdges = outEdges;
        }

        public int VertexCount => OutEdges?.Length ?? 0;

        public int EdgeCount => Endpoints?.Length / 2 ?? 0;

        private int[] Endpoints { get; }

        private ArrayBuilder<int>[] OutEdges { get; }

        public bool TryGetSource(int edge, out int source)
        {
            int edgeCount = EdgeCount;

            if ((uint)edge >= (uint)edgeCount)
            {
                source = -1;
                return false;
            }

            Assert(Endpoints != null, "Endpoints != null");
            source = Endpoints[edgeCount + edge];
            return true;
        }

        public bool TryGetTarget(int edge, out int target)
        {
            if ((uint)edge >= (uint)EdgeCount)
            {
                target = -1;
                return false;
            }

            Assert(Endpoints != null, "Endpoints != null");
            target = Endpoints[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out ArrayPrefixEnumerator<int> outEdges)
        {
            if ((uint)vertex >= (uint)VertexCount)
            {
                outEdges = new ArrayPrefixEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0);
                return false;
            }

            Assert(OutEdges != null, "OutEdges != null");
            if (OutEdges[vertex].Buffer == null)
            {
                outEdges = new ArrayPrefixEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0);
                return false;
            }

            outEdges = new ArrayPrefixEnumerator<int>(OutEdges[vertex].Buffer, OutEdges[vertex].Count);
            return true;
        }

        public bool Equals(JaggedAdjacencyListGraph other)
        {
            return Equals(Endpoints, other.Endpoints) && Equals(OutEdges, other.OutEdges);
        }

        public override bool Equals(object obj)
        {
            if (obj is JaggedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return unchecked(((Endpoints?.GetHashCode() ?? 0) * 397) ^ (OutEdges?.GetHashCode() ?? 0));
        }
    }
}
