namespace Ubiquitous.Internal
{
    using System;
    using Scg = System.Collections.Generic;

    internal static class QueueCache<T>
    {
        [ThreadStatic] private static WeakReference<Scg.Queue<T>> s_cachedInstanceReference;

        private static WeakReference<Scg.Queue<T>> CachedInstanceReference => s_cachedInstanceReference
            ?? (s_cachedInstanceReference = new WeakReference<Scg.Queue<T>>(null, false));

        public static Scg.Queue<T> Acquire(int desiredCapacity = 0)
        {
            if (desiredCapacity > 0)
                return new Scg.Queue<T>(desiredCapacity);

            bool isCached = CachedInstanceReference.TryGetTarget(out Scg.Queue<T> result);
            CachedInstanceReference.SetTarget(null);
            return isCached ? result : new Scg.Queue<T>();
        }

        public static void Release(Scg.Queue<T> queue)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));

            queue.Clear();
            CachedInstanceReference.SetTarget(queue);
        }
    }
}
