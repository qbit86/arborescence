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

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; ++i)
            {
                if (_items[i] != 0)
                    yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<int>.Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[item] = 1;
        }

        public void ExceptWith(IEnumerable<int> other) => throw new NotSupportedException();

        public void IntersectWith(IEnumerable<int> other) => throw new NotSupportedException();

        public bool IsProperSubsetOf(IEnumerable<int> other) => throw new NotSupportedException();

        public bool IsProperSupersetOf(IEnumerable<int> other) => throw new NotSupportedException();

        public bool IsSubsetOf(IEnumerable<int> other) => throw new NotSupportedException();

        public bool IsSupersetOf(IEnumerable<int> other) => throw new NotSupportedException();

        public bool Overlaps(IEnumerable<int> other) => throw new NotSupportedException();

        public bool SetEquals(IEnumerable<int> other) => throw new NotSupportedException();

        public void SymmetricExceptWith(IEnumerable<int> other) => throw new NotSupportedException();

        public void UnionWith(IEnumerable<int> other) => throw new NotSupportedException();

        bool ISet<int>.Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            bool result = _items[item] == 0;
            _items[item] = 1;
            return result;
        }

        public void Clear() => Array.Clear(_items, 0, _items.Length);

        public bool Contains(int item)
        {
            if (unchecked((uint)item >= (uint)_items.Length))
                return false;

            return _items[item] != 0;
        }

        public void CopyTo(int[] array, int arrayIndex) => throw new NotSupportedException();

        public bool Remove(int item)
        {
            if (unchecked((uint)item >= (uint)_items.Length))
                return false;

            _items[item] = 0;
            return true;
        }

        public int Count => throw new NotSupportedException();

        public bool IsReadOnly => false;
    }
}
