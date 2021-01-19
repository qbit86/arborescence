namespace Arborescence.Internal
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal struct MinHeap<TElement, TPriority, TPriorityMap, TIndexInHeapMap, TPriorityComparer> : IDisposable
        where TPriorityMap : IReadOnlyDictionary<TElement, TPriority>
        where TIndexInHeapMap : IDictionary<TElement, int>
        where TPriorityComparer : IComparer<TPriority>
    {
        private const int Arity = 4;
        private const int DefaultCapacity = 4;

        private TElement[] _arrayFromPool;
        private int _count;
        private readonly TPriorityMap _priorityByElement;
        private readonly TIndexInHeapMap _indexInHeapByElement;
        private readonly TPriorityComparer _priorityComparer;

        internal MinHeap(
            TPriorityMap priorityByElement, TIndexInHeapMap indexInHeapByElement, TPriorityComparer priorityComparer)
        {
            if (priorityByElement == null)
                throw new ArgumentNullException(nameof(priorityByElement));

            if (indexInHeapByElement == null)
                throw new ArgumentNullException(nameof(indexInHeapByElement));

            if (priorityComparer == null)
                throw new ArgumentNullException(nameof(priorityComparer));

            _arrayFromPool = Array.Empty<TElement>();
            _count = 0;
            _priorityByElement = priorityByElement;
            _indexInHeapByElement = indexInHeapByElement;
            _priorityComparer = priorityComparer;
        }

        private static ArrayPool<TElement> Pool => ArrayPool<TElement>.Shared;

        public void Dispose()
        {
            TElement[] arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool != null)
                Pool.Return(arrayFromPool, ShouldClear());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ShouldClear()
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1
            return RuntimeHelpers.IsReferenceOrContainsReferences<TElement>();
#else
            return true;
#endif
        }
    }
}
