namespace Ubiquitous.Collections
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct QueueAdapter<T> : IContainer<T>
    {
        private readonly Queue<T> _queue;

        public QueueAdapter(Queue<T> queue)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public bool IsEmpty => _queue.Count == 0;

        public void Add(T item) => _queue.Enqueue(item);

        public bool TryTake(out T result)
        {
            if (_queue.Count > 0)
            {
                result = _queue.Dequeue();
                return true;
            }

            result = default!;
            return false;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
