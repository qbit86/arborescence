namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Primitives;
    using static TryHelpers;

    public readonly partial struct Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> :
        IReadOnlyDictionary<int, TValue>,
        IEquatable<Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>>
        where TValueList : IReadOnlyList<TValue>
        where TAbsencePolicy : IEquatable<TValue>
    {
        private readonly TValueList _items;
        private readonly TAbsencePolicy _absencePolicy;

        internal Int32ReadOnlyDictionary(TValueList items, TAbsencePolicy absencePolicy)
        {
            _items = items;
            _absencePolicy = absencePolicy;
        }

        public int Count
        {
            get
            {
                Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
                return self._items is null ? 0 : self.CountUnchecked;
            }
        }

        private int CountUnchecked => _items.Count;

        public bool ContainsKey(int key)
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            return unchecked((uint)key < (uint)self._items.Count) && !_absencePolicy.Equals(self._items[key]);
        }

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            if (self._items is null)
                return None(out value);
            if (unchecked((uint)key >= (uint)self._items.Count))
                return None(out value);

            value = self._items[key];
            return !_absencePolicy.Equals(value);
        }

        public TValue this[int key]
        {
            get
            {
                if (!TryGetValueCore(key, out TValue? value))
                    ThrowHelper.ThrowKeyNotFoundException(key);
                return value;
            }
        }
    }
}
