#if !NETCOREAPP2_0_OR_GREATER
namespace System.Collections.Generic
{
    using ComponentModel;

    internal static class KeyValuePairExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Deconstruct<TKey, TValue>(
            this KeyValuePair<TKey, TValue> source, out TKey key, out TValue value)
        {
            key = source.Key;
            value = source.Value;
        }
    }
}
#endif
