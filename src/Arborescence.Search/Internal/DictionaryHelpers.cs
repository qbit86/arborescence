#if ASTAR_SUPPORTED
namespace Arborescence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class DictionaryHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Put<TKey, TValue, TDictionary>(this TDictionary dictionary, TKey key, TValue value)
            where TDictionary : IDictionary<TKey, TValue> =>
            dictionary[key] = value;
    }
}
#endif
