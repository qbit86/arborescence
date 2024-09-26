namespace Arborescence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class DictionaryHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TValue GetValueOrDefault<TKey, TValue, TDictionary>(
            this TDictionary dictionary, TKey key, TValue defaultValue)
            where TDictionary : IDictionary<TKey, TValue> =>
            dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }
}
