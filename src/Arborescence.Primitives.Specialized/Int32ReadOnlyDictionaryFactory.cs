namespace Arborescence
{
    using System.Collections.Generic;
    using Primitives;

    public static class Int32ReadOnlyDictionaryFactory<TValue>
    {
        public static Int32ReadOnlyDictionary<TValue, TValueList> Create<TValueList>(TValueList items)
            where TValueList : IReadOnlyList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items);
        }
    }
}
