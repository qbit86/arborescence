#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
namespace Arborescence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    internal static class ReadOnlyDictionaryHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetValue<TKey, TValue, TReadOnlyDictionary>(
            this TReadOnlyDictionary dictionary, TKey key, [MaybeNullWhen(false)] out TValue value)
            where TReadOnlyDictionary : IReadOnlyDictionary<TKey, TValue> =>
            dictionary.TryGetValue(key, out value);
    }
}
#endif
