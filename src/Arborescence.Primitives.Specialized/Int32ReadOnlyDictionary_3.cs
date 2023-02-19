namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly partial struct Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> :
        IReadOnlyDictionary<int, TValue>,
        IEquatable<Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>>
        where TValueList : IReadOnlyList<TValue>
        where TAbsencePolicy : IEquatable<TValue>
    {
        private readonly TValue[] _items;
        private readonly TAbsencePolicy _absencePolicy;

        internal Int32ReadOnlyDictionary(TValue[] items, TAbsencePolicy absencePolicy)
        {
            _items = items;
            _absencePolicy = absencePolicy;
        }

        public int Count => throw new NotImplementedException();

        public bool ContainsKey(int key) => throw new NotImplementedException();

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) =>
            throw new NotImplementedException();

#if NET461 || NETSTANDARD2_0 || NETSTANDARD2_1
        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) => TryGetValue(key, out value!);
#endif

        public TValue this[int key] => throw new NotImplementedException();
    }
}
