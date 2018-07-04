namespace Ubiquitous.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal abstract class QueuePool<T>
    {
        [ThreadStatic] private static WeakReference<QueuePool<T>> s_reference;

        private static WeakReference<QueuePool<T>> Reference =>
            s_reference ?? (s_reference = new WeakReference<QueuePool<T>>(null, false));

        public static QueuePool<T> Shared
        {
            get
            {
                if (Reference.TryGetTarget(out QueuePool<T> pool))
                    return pool;

                pool = new DefaultQueuePool<T>();
                Reference.SetTarget(pool);
                return pool;
            }
        }

        public abstract Queue<T> Rent();

        public abstract void Return(Queue<T> queue);
    }

    internal sealed class DefaultQueuePool<T> : QueuePool<T>
    {
        private Queue<T> _sharedInstance;

        public override Queue<T> Rent()
        {
            Queue<T> result = Interlocked.Exchange(ref _sharedInstance, null);
            if (result == null)
                return new Queue<T>();

            return result;
        }

        public override void Return(Queue<T> queue)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));

            queue.Clear();

            _sharedInstance = queue;
        }
    }
}
