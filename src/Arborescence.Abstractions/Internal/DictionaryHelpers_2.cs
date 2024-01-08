#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
namespace Arborescence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class DictionaryHelpers<TKey, TValue>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ICollection<TKey> GetKeys<TDictionary>(TDictionary dictionary)
            where TDictionary : IDictionary<TKey, TValue> =>
            dictionary.Keys;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ICollection<TValue> GetValues<TDictionary>(TDictionary dictionary)
            where TDictionary : IDictionary<TKey, TValue> =>
            dictionary.Values;
    }
}
#endif
