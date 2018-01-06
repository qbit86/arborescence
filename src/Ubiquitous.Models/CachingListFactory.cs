namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public struct CachingListFactory<TContext, TElem> : IFactory<TContext, List<TElem>>
    {
        private List<TElem> _cachedList;

        private int Capacity { get; }

        public CachingListFactory(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _cachedList = null;

            Capacity = capacity;
        }

        public List<TElem> Acquire(TContext context)
        {
            if (_cachedList == null)
                return new List<TElem>(Capacity);

            List<TElem> result = Interlocked.Exchange(ref _cachedList, null);
            return result ?? new List<TElem>(Capacity);
        }

        public void Release(TContext context, List<TElem> value)
        {
            value?.Clear();

            _cachedList = value;
        }

        public void Warmup()
        {
            if (_cachedList == null)
                _cachedList = new List<TElem>(Capacity);
            else if (_cachedList.Capacity < Capacity)
                _cachedList.Capacity = Capacity;
        }
    }
}
