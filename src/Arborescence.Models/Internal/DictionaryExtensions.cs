namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal static class DictionaryExtensions
    {
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
