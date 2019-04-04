namespace Ubiquitous.Traversal
{
    using System;
    using System.Text;

    public readonly struct IndexedDfsStep : IEquatable<IndexedDfsStep>
    {
        private const uint KindMask = 0x7Fu;

        private const uint ReverseMask = 0x80u;

        private readonly uint _storage;

        public IndexedDfsStep(DfsStepKind kind, int value, bool isReversed = false)
        {
            if ((uint)kind > 0x7Fu)
                throw new ArgumentOutOfRangeException(nameof(kind));

            if ((uint)value > 0x00FFFFFFu)
                throw new ArgumentOutOfRangeException(nameof(value));

            _storage = ((uint)value << 8) | (KindMask & (uint)kind);
            if (isReversed)
                _storage |= ReverseMask;
        }

        public const int UpperBound = 0x01000000;

        public DfsStepKind Kind => (DfsStepKind)(KindMask & _storage);

        public int Value => (int)(_storage >> 8);

        public bool IsReversed => (ReverseMask & _storage) != 0;

        public bool Equals(IndexedDfsStep other)
        {
            return _storage == other._storage;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append('[');

            if (IsReversed)
                s.Append('-');

            s.Append(Kind);
            s.Append(", ");
            s.Append(Value);
            s.Append(']');

            return s.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is IndexedDfsStep other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _storage.GetHashCode();
        }

        public static bool operator ==(IndexedDfsStep left, IndexedDfsStep right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IndexedDfsStep left, IndexedDfsStep right)
        {
            return !left.Equals(right);
        }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedDfsStepPolicy
        : IStepPolicy<DfsStepKind, int, int, IndexedDfsStep>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public IndexedDfsStep CreateVertexStep(DfsStepKind kind, int vertex)
        {
            return new IndexedDfsStep(kind, vertex);
        }

        public IndexedDfsStep CreateEdgeStep(DfsStepKind kind, int edge)
        {
            return new IndexedDfsStep(kind, edge);
        }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedUndirectedDfsStepPolicy
        : IUndirectedStepPolicy<DfsStepKind, int, int, IndexedDfsStep>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public IndexedDfsStep CreateVertexStep(DfsStepKind kind, int vertex)
        {
            return new IndexedDfsStep(kind, vertex);
        }

        public IndexedDfsStep CreateEdgeStep(DfsStepKind kind, int edge, bool isReversed)
        {
            return new IndexedDfsStep(kind, edge, isReversed);
        }
    }
}
