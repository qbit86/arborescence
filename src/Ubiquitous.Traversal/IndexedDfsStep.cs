namespace Ubiquitous.Traversal
{
    using System;

    public readonly struct IndexedDfsStep : IEquatable<IndexedDfsStep>
    {
        private const uint KindMask = 0x000000FFu;

        private readonly uint _storage;

        public IndexedDfsStep(DfsStepKind kind, int value)
        {
            if ((uint)kind > 0x7Fu)
                throw new ArgumentOutOfRangeException(nameof(kind));

            if ((uint)value > 0x00FFFFFFu)
                throw new ArgumentOutOfRangeException(nameof(value));

            _storage = ((uint)value << 8) | (KindMask & (uint)kind);
        }

        public DfsStepKind Kind => (DfsStepKind)(KindMask & _storage);

        public int Value => (int)(_storage >> 8);

        public bool Equals(IndexedDfsStep other)
        {
            return _storage == other._storage;
        }

        public override string ToString()
        {
            return $"[{Kind}, {Value}]";
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
}
