namespace Arborescence.Internal
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
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

        internal int Count => _count;

        public void Dispose()
        {
            TElement[] arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool != null)
                Pool.Return(arrayFromPool, ShouldClear());
        }

        private static int GetParent(int index) => (index - 1) / Arity;

        private int Compare(TElement left, TElement right)
        {
            bool hasLeft = _priorityByElement.TryGetValue(left, out TPriority leftPriority);
            bool hasRight = _priorityByElement.TryGetValue(right, out TPriority rightPriority);
            if (!hasLeft)
                return hasRight ? 1 : 0;

            if (!hasRight)
                return -1;

            return _priorityComparer.Compare(leftPriority, rightPriority);
        }

        [Conditional("DEBUG")]
        private void VerifyHeap()
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert((uint)count <= (uint)array.Length, "(uint)count <= (uint)array.Length");

            for (int i = 1; i < count; ++i)
            {
                int order = Compare(array[i], array[GetParent(i)]);
                if (order < 0)
                    throw new InvalidOperationException("Element is smaller than its parent.");
            }
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
