namespace Arborescence.Internal
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal struct MinHeap<
            TElement, TPriority, TPriorityMap, TIndexMap, TPriorityComparer, TPriorityMapPolicy, TIndexMapPolicy>
        : IDisposable
        where TPriorityComparer : IComparer<TPriority>
        where TPriorityMapPolicy : IReadOnlyMapPolicy<TPriorityMap, TElement, TPriority>
        where TIndexMapPolicy : IMapPolicy<TIndexMap, TElement, int>
    {
        private const int DefaultCapacity = 4;

        private TElement[] _arrayFromPool;
        private readonly TPriorityComparer _priorityComparer;
        private int _count;

        public MinHeap(TPriorityComparer priorityComparer)
        {
            if (priorityComparer is null)
                throw new ArgumentNullException(nameof(priorityComparer));

            _arrayFromPool = Array.Empty<TElement>();
            _priorityComparer = priorityComparer;
            _count = 0;
        }

        private static ArrayPool<TElement> Pool => ArrayPool<TElement>.Shared;

        public void Dispose()
        {
            TElement[] arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool is null)
                return;

            Pool.Return(arrayFromPool, ShouldClear());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ShouldClear()
        {
#if NETSTANDARD2_1 || NETCOREAPP2_0
            return RuntimeHelpers.IsReferenceOrContainsReferences<TElement>();
#else
            return true;
#endif
        }
    }
}
