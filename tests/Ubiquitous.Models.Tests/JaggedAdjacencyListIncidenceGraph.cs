namespace Ubiquitous.Models
{
    using System;
    using static System.Diagnostics.Debug;

    internal readonly struct JaggedAdjacencyListIncidenceGraph : IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>,
        IEquatable<JaggedAdjacencyListIncidenceGraph>
    {
        // Layout: endpoints start with targets, then sources follow.
        internal JaggedAdjacencyListIncidenceGraph(int[] endpoints, ArrayPrefix<int>[] outEdges)
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

        private ArrayPrefix<int>[] OutEdges { get; }

        public bool TryGetTail(int edge, out int source)
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

        public bool TryGetHead(int edge, out int target)
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

        public ArrayPrefixEnumerator<int> EnumerateOutEdges(int vertex)
        {
            if ((uint)vertex >= (uint)VertexCount)
                return new ArrayPrefixEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0);

            Assert(OutEdges != null, "OutEdges != null");
            if (OutEdges[vertex].Array is null)
                return new ArrayPrefixEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0);

            return new ArrayPrefixEnumerator<int>(OutEdges[vertex].Array, OutEdges[vertex].Count);
        }

        public bool Equals(JaggedAdjacencyListIncidenceGraph other)
        {
            return Equals(Endpoints, other.Endpoints) && Equals(OutEdges, other.OutEdges);
        }

        public override bool Equals(object obj)
        {
            return obj is JaggedAdjacencyListIncidenceGraph other && Equals(other);
        }

        public override int GetHashCode()
        {
            return unchecked((Endpoints?.GetHashCode() ?? 0) * 397) ^ (OutEdges?.GetHashCode() ?? 0);
        }
    }
}
