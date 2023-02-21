namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
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

        public int Count => (_items?.Count).GetValueOrDefault();

        public bool ContainsKey(int key)
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            if (self._items is not { } items)
                return false;
            return unchecked((uint)key < (uint)items.Count) && !self._absencePolicy.Equals(items[key]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            if (self._items is not { } items)
                return None(out value);
            if (unchecked((uint)key >= (uint)items.Count))
                return None(out value);

            value = items[key];
            return !self._absencePolicy.Equals(value);
        }

        public TValue this[int key] => TryGetValueCore(key, out TValue? value)
            ? value
            : ThrowHelper.ThrowKeyNotFoundException<TValue>(key);
    }
}
