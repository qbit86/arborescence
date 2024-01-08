#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a formal implementation for the <see cref="IReadOnlyDictionary{TKey,TValue}"/> methods
    /// that are not used in the algorithms.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the read-only dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the read-only dictionary.</typeparam>
    public interface IPartialReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => TryGetValue(key, out _);

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out TValue? value))
                    ThrowHelper.ThrowKeyNotFoundException();
                return value;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                ThrowHelper.ThrowNotSupportedException();
                return default;
            }
        }
    }
}
#endif
