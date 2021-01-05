namespace Arborescence.Models
{
    using System.Collections;
    using System.Collections.Generic;

#if NET5
    public readonly struct IndexedSet : ISet<int>, IReadOnlySet<int>
#else
    public readonly struct IndexedSet : ISet<int>
#endif
    {
        private readonly byte[] _items;

        public IndexedSet(byte[] items) => _items = items;

        public IEnumerator<int> GetEnumerator() => throw new System.NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<int>.Add(int item) => throw new System.NotImplementedException();

        public void ExceptWith(IEnumerable<int> other) => throw new System.NotImplementedException();

        public void IntersectWith(IEnumerable<int> other) => throw new System.NotImplementedException();

        public bool IsProperSubsetOf(IEnumerable<int> other) => throw new System.NotImplementedException();

        public bool IsProperSupersetOf(IEnumerable<int> other) => throw new System.NotImplementedException();

        public bool IsSubsetOf(IEnumerable<int> other) => throw new System.NotImplementedException();

        public bool IsSupersetOf(IEnumerable<int> other) => throw new System.NotImplementedException();

        public bool Overlaps(IEnumerable<int> other) => throw new System.NotImplementedException();

        public bool SetEquals(IEnumerable<int> other) => throw new System.NotImplementedException();

        public void SymmetricExceptWith(IEnumerable<int> other) => throw new System.NotImplementedException();

        public void UnionWith(IEnumerable<int> other) => throw new System.NotImplementedException();

        bool ISet<int>.Add(int item) => throw new System.NotImplementedException();

        public void Clear() => throw new System.NotImplementedException();

        public bool Contains(int item) => throw new System.NotImplementedException();

        public void CopyTo(int[] array, int arrayIndex) => throw new System.NotImplementedException();

        public bool Remove(int item) => throw new System.NotImplementedException();

        public int Count => throw new System.NotImplementedException();

        public bool IsReadOnly => throw new System.NotImplementedException();
    }
}
