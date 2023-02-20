namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;
    using static TryHelpers;

    public readonly partial struct Int32ReadOnlyDictionary<TValue, TValueList> :
        IReadOnlyDictionary<int, TValue>,
        IEquatable<Int32ReadOnlyDictionary<TValue, TValueList>>
        where TValueList : IReadOnlyList<TValue>
    {
        private readonly TValueList _items;

        internal Int32ReadOnlyDictionary(TValueList items) => _items = items;

        public int Count => (_items?.Count).GetValueOrDefault();

        public bool ContainsKey(int key) => unchecked((uint)key < (uint)Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) => TryGetValueCore(key, out value);

        private bool TryGetValueCore(int key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_items is not { } items)
                return None(out value);
            return unchecked((uint)key >= (uint)items.Count)
                ? None(out value)
                : Some(items[key], out value);
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
