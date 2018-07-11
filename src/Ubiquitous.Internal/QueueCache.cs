namespace Ubiquitous.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class QueueCache<T>
    {
        [ThreadStatic] private static WeakReference<Queue<T>> s_cachedInstanceReference;

        private static WeakReference<Queue<T>> CachedInstanceReference => s_cachedInstanceReference
            ?? (s_cachedInstanceReference = new WeakReference<Queue<T>>(null, false));

        public static Queue<T> Acquire(int desiredCapacity = 0)
        {
            if (desiredCapacity > 0)
                return new Queue<T>(desiredCapacity);

            bool isCached = CachedInstanceReference.TryGetTarget(out Queue<T> result);
            CachedInstanceReference.SetTarget(null);
            return isCached ? result : new Queue<T>();
        }

        public static void Release(Queue<T> queue)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));

            queue.Clear();
            CachedInstanceReference.SetTarget(queue);
        }
    }
}
