namespace Ubiquitous
{
    using System.Collections.Generic;
    using IndexedDfsStep = Traversal.DfsStep<int>;

    internal sealed class IndexedDfsStepEqualityComparer : IEqualityComparer<IndexedDfsStep>
    {
        internal static IndexedDfsStepEqualityComparer Default { get; } = new IndexedDfsStepEqualityComparer();

        public bool Equals(IndexedDfsStep x, IndexedDfsStep y) => x.Equals(y);

        public int GetHashCode(IndexedDfsStep obj) => obj.GetHashCode();
    }
}
