#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
namespace Arborescence
{
    using System.Collections.Generic;

    internal static class ReadOnlyCollectionHelpers<T>
    {
        internal static int GetCount<TReadOnlyCollection>(TReadOnlyCollection collection)
            where TReadOnlyCollection : IReadOnlyCollection<T> => collection.Count;
    }
}
#endif
