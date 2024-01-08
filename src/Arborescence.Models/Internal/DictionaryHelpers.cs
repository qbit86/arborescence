namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class DictionaryHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryAddStrict<TKey, TValue, TDictionary>(
            this TDictionary dictionary, TKey key, TValue value)
            where TDictionary : IDictionary<TKey, TValue>
        {
            if (dictionary.ContainsKey(key))
                return false;

            dictionary.Add(key, value);
            return true;
        }
    }
}
