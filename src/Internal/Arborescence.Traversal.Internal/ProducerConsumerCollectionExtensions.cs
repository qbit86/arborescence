namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Concurrent;

    internal static class ProducerConsumerCollectionExtensions
    {
        internal static void AddOrThrow<T, TCollection>(this TCollection collection, T element)
            where TCollection : IProducerConsumerCollection<T>
        {
            if (!collection.TryAdd(element))
                throw new InvalidOperationException("TryAdd");
        }
    }
}
