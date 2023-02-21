namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public readonly partial struct Int32Dictionary<TValue, TValueList, TAbsenceComparer> :
        IReadOnlyDictionary<int, TValue>, IDictionary<int, TValue>,
        IEquatable<Int32Dictionary<TValue, TValueList, TAbsenceComparer>>
        where TValueList : IList<TValue>
        where TAbsenceComparer : IEqualityComparer<TValue>
    {
        private readonly TValueList _items;
        private readonly TValue _absenceMarker;
        private readonly TAbsenceComparer _absenceComparer;

        internal Int32Dictionary(TValueList items, TValue absenceMarker, TAbsenceComparer absenceComparer)
        {
            _items = items;
            _absenceMarker = absenceMarker;
            _absenceComparer = absenceComparer;
        }

        public int Count => (_items?.Count).GetValueOrDefault();

        public void Add(int key, TValue value) => throw new NotImplementedException();

        public bool ContainsKey(int key) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value) =>
            throw new NotImplementedException();

        public TValue this[int key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
