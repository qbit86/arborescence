namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    public readonly partial struct Int32Dictionary<TValue, TValueList, TAbsenceComparer> :
        IReadOnlyDictionary<int, TValue>, IDictionary<int, TValue>,
        IEquatable<Int32Dictionary<TValue, TValueList, TAbsenceComparer>>
        where TValueList : IList<TValue>
        where TAbsenceComparer : IEqualityComparer<TValue>
    {
        private readonly TValueList _items;
        private readonly TAbsenceComparer _absenceComparer;
        private readonly TValue? _absenceMarker;

        internal Int32Dictionary(TValueList items, TAbsenceComparer absenceComparer, TValue? absenceMarker)
        {
            _items = items;
            _absenceComparer = absenceComparer;
            _absenceMarker = absenceMarker;
        }

        public int Count => (_items?.Count).GetValueOrDefault();

        public void Add(int key, TValue value) => throw new NotImplementedException();

        public bool ContainsKey(int key)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return unchecked((uint)key < (uint)self.Count) &&
                !self._absenceComparer.Equals(self._items[key], self._absenceMarker!);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._items is not { } items)
                return None(out value);
            if (unchecked((uint)key >= (uint)items.Count))
                return None(out value);

            value = items[key];
            return !self._absenceComparer.Equals(value, _absenceMarker!);
        }

        private void Put(int key, TValue value)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            TValueList items = _items;
            int count = items.Count;
            if (key < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));
                return;
            }

            if (key < count)
            {
                items[key] = value;
                return;
            }

            int absentCount = key - count;
            for (int i = 0; i < absentCount; ++i)
                items.Add(self._absenceMarker!);
            items.Add(value);
        }

        public TValue this[int key]
        {
            get => TryGetValueCore(key, out TValue? value)
                ? value
                : ThrowHelper.ThrowKeyNotFoundException<TValue>(key);
            set => Put(key, value);
        }
    }
}
