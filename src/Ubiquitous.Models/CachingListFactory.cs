namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public struct CachingListFactory<TContext, TElem> : IFactory<TContext, List<TElem>>
    {
        private List<TElem> _cachedInstance;

        private int Capacity { get; }

        public CachingListFactory(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _cachedInstance = null;

            Capacity = capacity;
        }

        public List<TElem> Acquire(TContext context)
        {
            if (_cachedInstance == null)
                return new List<TElem>(Capacity);

            List<TElem> result = Interlocked.Exchange(ref _cachedInstance, null);
            return result ?? new List<TElem>(Capacity);
        }

        public void Release(TContext context, List<TElem> value)
        {
            value?.Clear();

            _cachedInstance = value;
        }

        public void Warmup()
        {
            if (_cachedInstance == null)
                _cachedInstance = new List<TElem>(Capacity);
            else if (_cachedInstance.Capacity < Capacity)
                _cachedInstance.Capacity = Capacity;
        }
    }
}
