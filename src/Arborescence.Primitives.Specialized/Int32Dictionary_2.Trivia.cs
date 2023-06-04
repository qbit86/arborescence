namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Primitives;

    partial struct Int32Dictionary<TValue, TValueList>
    {
        public bool IsReadOnly => false;

        public IEnumerable<int> Keys
        {
            get
            {
                int count = (_values?.Count).GetValueOrDefault();
                return count is 0 ? Enumerable.Empty<int>() : Enumerable.Range(0, count);
            }
        }

        public IEnumerable<TValue> Values => _values ?? Enumerable.Empty<TValue>();

        ICollection<int> IDictionary<int, TValue>.Keys => _values is not { Count: > 0 }
            ? Array.Empty<int>()
            : ThrowHelper.ThrowNotSupportedException<ICollection<int>>();

        ICollection<TValue> IDictionary<int, TValue>.Values => _values ?? (ICollection<TValue>)Array.Empty<TValue>();

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator() =>
            _values is not { Count: > 0 } values
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : GetEnumeratorIterator(values);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator(TValueList values)
        {
            int count = values.Count;
            for (int key = 0; key < count; ++key)
                yield return new(key, values[key]);
        }

        public void Add(KeyValuePair<int, TValue> item) => Add(item.Key, item.Value);

        public void Clear() => _values?.Clear();

        public bool Contains(KeyValuePair<int, TValue> item) =>
            TryGetValueCore(item.Key, out TValue? value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                return;
            }

            if (unchecked((uint)arrayIndex > (uint)array.Length))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(arrayIndex));

            if (_values is not { } values)
                return;
            Span<KeyValuePair<int, TValue>> destination = array.AsSpan(arrayIndex);
            int count = values.Count;
            if (destination.Length < count)
                ThrowHelper.ThrowDestinationArrayTooSmallException();
            for (int i = 0; i < count; ++i)
                destination[i] = new(i, values[i]);
        }

        public bool Remove(KeyValuePair<int, TValue> item)
        {
            if (_values is not { } values)
                return false;
            int key = item.Key;
            int count = values.Count;
            if (unchecked((uint)key >= (uint)count))
                return false;
            if (!EqualityComparer<TValue>.Default.Equals(values[key], item.Value))
                return false;
            if (key != count - 1)
                return ThrowHelper.ThrowNotSupportedException<bool>();
            values.RemoveAt(key);
            return true;
        }

        public bool Remove(int key)
        {
            if (_values is not { } values)
                return false;
            int count = values.Count;
            if (unchecked((uint)key >= (uint)count))
                return false;
            if (key != count - 1)
                return ThrowHelper.ThrowNotSupportedException<bool>();
            values.RemoveAt(key);
            return true;
        }

        bool IDictionary<int, TValue>.TryGetValue(int key, out TValue value) => TryGetValueCore(key, out value!);

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public bool Equals(Int32Dictionary<TValue, TValueList> other) =>
            EqualityComparer<TValueList>.Default.Equals(_values, other._values);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Dictionary<TValue, TValueList> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<TValueList>.Default.GetHashCode(_values);

        public static bool operator ==(
            Int32Dictionary<TValue, TValueList> left, Int32Dictionary<TValue, TValueList> right) => left.Equals(right);

        public static bool operator !=(
            Int32Dictionary<TValue, TValueList> left, Int32Dictionary<TValue, TValueList> right) => !left.Equals(right);
    }
}
