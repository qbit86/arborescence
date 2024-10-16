#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a formal implementation for the <see cref="IDictionary{TKey,TValue}"/> methods
    /// that are not used in the algorithms.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the read-only dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the read-only dictionary.</typeparam>
    public interface IPartialDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => ThrowHelper.ThrowNotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ThrowHelper.ThrowNotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count =>
            ReadOnlyCollectionHelpers<KeyValuePair<TKey, TValue>>.GetCount(this);

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        bool IDictionary<TKey, TValue>.ContainsKey(TKey key) =>
            ReadOnlyDictionaryHelpers<TValue>.ContainsKey(this, key);

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            ReadOnlyDictionaryHelpers.TryGetValue(this, key, out value!);

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get => ReadOnlyDictionaryHelpers<TValue>.GetValue(this, key);
            set => Put(key, value);
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) =>
            ReadOnlyDictionaryHelpers<TValue>.TryGetValue(this, key, out _);

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                if (!ReadOnlyDictionaryHelpers<TValue>.TryGetValue(this, key, out var value))
                    ThrowHelper.ThrowKeyNotFoundException();
                return value!;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => DictionaryHelpers<TKey, TValue>.GetKeys(this);

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => DictionaryHelpers<TKey, TValue>.GetValues(this);

        /// <summary>
        /// Sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to set.</param>
        /// <param name="value">The element with the specified key.</param>
        void Put(TKey key, TValue value);
    }
}
#endif
