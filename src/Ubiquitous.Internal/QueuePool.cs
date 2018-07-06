namespace Ubiquitous.Internal
{
    using System;
    using System.Collections.Generic;

    internal abstract class QueuePool<T>
    {
        [ThreadStatic] private static QueuePool<T> s_sharedInstance;

        public static QueuePool<T> Shared => s_sharedInstance ?? (s_sharedInstance = new DefaultQueuePool<T>());

        public abstract Queue<T> Rent(int desiredCapacity);

        public abstract void Return(Queue<T> queue);
    }

    internal sealed class DefaultQueuePool<T> : QueuePool<T>
    {
        private WeakReference<Queue<T>> _cachedInstanceReference;

        private WeakReference<Queue<T>> CachedInstanceReference => _cachedInstanceReference
            ?? (_cachedInstanceReference = new WeakReference<Queue<T>>(null, false));

        public override Queue<T> Rent(int desiredCapacity)
        {
            if (desiredCapacity > 0)
                return new Queue<T>(desiredCapacity);

            bool isCached = CachedInstanceReference.TryGetTarget(out Queue<T> result);
            return isCached ? result : new Queue<T>();
        }

        public override void Return(Queue<T> queue)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));

            queue.Clear();
            CachedInstanceReference.SetTarget(queue);
        }
    }
}
