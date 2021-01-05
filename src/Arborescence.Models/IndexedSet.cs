namespace Arborescence.Models
{
    using System;
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

        public IEnumerator<int> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<int>.Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[item] = 1;
        }

        public void ExceptWith(IEnumerable<int> other) => throw new NotImplementedException();

        public void IntersectWith(IEnumerable<int> other) => throw new NotImplementedException();

        public bool IsProperSubsetOf(IEnumerable<int> other) => throw new NotImplementedException();

        public bool IsProperSupersetOf(IEnumerable<int> other) => throw new NotImplementedException();

        public bool IsSubsetOf(IEnumerable<int> other) => throw new NotImplementedException();

        public bool IsSupersetOf(IEnumerable<int> other) => throw new NotImplementedException();

        public bool Overlaps(IEnumerable<int> other) => throw new NotImplementedException();

        public bool SetEquals(IEnumerable<int> other) => throw new NotImplementedException();

        public void SymmetricExceptWith(IEnumerable<int> other) => throw new NotImplementedException();

        public void UnionWith(IEnumerable<int> other) => throw new NotImplementedException();

        bool ISet<int>.Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            bool result = _items[item] == 0;
            _items[item] = 1;
            return result;
        }

        public void Clear() => throw new NotImplementedException();

        public bool Contains(int item)
        {
            if (unchecked((uint)item >= (uint)_items.Length))
                return false;

            return _items[item] != 0;
        }

        public void CopyTo(int[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(int item) => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();
    }
}
