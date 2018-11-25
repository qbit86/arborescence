namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public readonly struct EdgeListIncidenceGraph :
        IIncidenceGraph<int, SourceTargetPair<int>, ArraySegmentEnumerator<SourceTargetPair<int>>>,
        IEquatable<EdgeListIncidenceGraph>
    {
        private readonly SourceTargetPair<int>[] _storage;

        internal EdgeListIncidenceGraph(int vertexUpperBound, SourceTargetPair<int>[] storage)
        {
            Assert(vertexUpperBound >= 0, "vertexUpperBound >= 0");
            Assert(storage != null, "storage != null");

            _storage = storage;
            VertexUpperBound = vertexUpperBound;
        }

        public int VertexUpperBound { get; }

        public bool TryGetSource(SourceTargetPair<int> edge, out int source)
        {
            if ((uint)edge.Source >= (uint)VertexUpperBound)
            {
                source = -1;
                return false;
            }

            if ((uint)edge.Target >= (uint)VertexUpperBound)
            {
                source = -2;
                return false;
            }

            source = edge.Source;
            return true;
        }

        public bool TryGetTarget(SourceTargetPair<int> edge, out int target)
        {
            if ((uint)edge.Source >= (uint)VertexUpperBound)
            {
                target = -1;
                return false;
            }

            if ((uint)edge.Target >= (uint)VertexUpperBound)
            {
                target = -2;
                return false;
            }

            target = edge.Target;
            return true;
        }

        public bool TryGetOutEdges(int vertex, out ArraySegmentEnumerator<SourceTargetPair<int>> edges)
        {
            throw new NotImplementedException();
        }

        public bool Equals(EdgeListIncidenceGraph other)
        {
            return VertexUpperBound == other.VertexUpperBound && _storage == other._storage;
        }

        public override bool Equals(object obj)
        {
            return obj is EdgeListIncidenceGraph other && Equals(other);
        }

        public override int GetHashCode()
        {
            return unchecked(VertexUpperBound * 397) ^ (_storage?.GetHashCode() ?? 0);
        }
    }
}
