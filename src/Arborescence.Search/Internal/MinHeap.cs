namespace Arborescence.Internal
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/detail/d_ary_heap.hpp

    internal struct MinHeap<
            TElement, TPriority, TPriorityMap, TIndexMap, TPriorityComparer, TPriorityMapPolicy, TIndexMapPolicy>
        : IDisposable
        where TPriorityComparer : IComparer<TPriority>
        where TPriorityMapPolicy : IReadOnlyMapPolicy<TPriorityMap, TElement, TPriority>
        where TIndexMapPolicy : IMapPolicy<TIndexMap, TElement, int>
    {
        private const int Arity = 4;
        private const int DefaultCapacity = 4;

        private TElement[] _arrayFromPool;
        private readonly TPriorityMap _priorityByElement;
        private readonly TIndexMap _indexByElement;
        private readonly TPriorityComparer _priorityComparer;
        private readonly TPriorityMapPolicy _priorityMapPolicy;
        private readonly TIndexMapPolicy _indexMapPolicy;
        private int _count;

        internal MinHeap(TPriorityMap priorityByElement, TIndexMap indexByElement,
            TPriorityComparer priorityComparer, TPriorityMapPolicy priorityMapPolicy, TIndexMapPolicy indexMapPolicy)
        {
            if (priorityByElement is null)
                throw new ArgumentNullException(nameof(priorityByElement));

            if (indexByElement is null)
                throw new ArgumentNullException(nameof(indexByElement));

            if (priorityComparer is null)
                throw new ArgumentNullException(nameof(priorityComparer));

            if (priorityMapPolicy is null)
                throw new ArgumentNullException(nameof(priorityMapPolicy));

            if (indexMapPolicy is null)
                throw new ArgumentNullException(nameof(indexMapPolicy));

            _arrayFromPool = Array.Empty<TElement>();
            _priorityByElement = priorityByElement;
            _indexByElement = indexByElement;
            _priorityComparer = priorityComparer;
            _priorityMapPolicy = priorityMapPolicy;
            _indexMapPolicy = indexMapPolicy;
            _count = 0;
        }

        private static ArrayPool<TElement> Pool => ArrayPool<TElement>.Shared;

        public void Dispose()
        {
            TElement[] arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool != null)
                Pool.Return(arrayFromPool, ShouldClear());
        }

        internal void Add(TElement element)
        {
            int count = _count;
            TElement[] array = _arrayFromPool;

            if ((uint)count < (uint)array.Length)
            {
                UncheckedAdd(element);
            }
            else
            {
                UncheckedGrow();
                UncheckedAdd(element);
            }

            VerifyHeap();
        }

        internal void AddOrUpdate(TElement element)
        {
            throw new NotImplementedException();
        }

        internal bool TryPeek(out TElement element)
        {
            if (_count == 0)
            {
                element = default;
                return false;
            }

            element = _arrayFromPool[0];
            return true;
        }

        internal bool TryTake()
        {
            int count = _count;

            if (count == 0)
                return false;

            throw new NotImplementedException();
        }

        private void UncheckedAdd(TElement element)
        {
            int count = _count;
            TElement[] array = _arrayFromPool;
            Debug.Assert((uint)count < (uint)array.Length, "(uint)count < (uint)array.Length");

            array[count] = element;
            _count = count + 1;
            _indexMapPolicy.AddOrUpdate(_indexByElement, element, count);
            HeapifyUp(count);
        }

        private void UncheckedGrow()
        {
            int count = _count;
            TElement[] arrayFromPool = _arrayFromPool;
            Debug.Assert((uint)count == (uint)arrayFromPool.Length, "(uint)count == (uint)arrayFromPool.Length");

            int newCapacity = count > 0 ? count << 1 : DefaultCapacity;
            TElement[] newArrayFromPool = Pool.Rent(newCapacity);
            if (count > 0)
                Array.Copy(arrayFromPool, newArrayFromPool, count);

            _arrayFromPool = newArrayFromPool;
            Pool.Return(arrayFromPool, ShouldClear());
        }

        private static int GetParentIndex(int index) => (index - 1) / Arity;

        private void Swap(int leftIndex, int rightIndex)
        {
            Debug.Assert(unchecked((uint)leftIndex < (uint)_count), "(uint)leftIndex < (uint)_count");
            Debug.Assert(unchecked((uint)rightIndex < (uint)_count), "(uint)rightIndex < (uint)_count");

            TElement left = _arrayFromPool[leftIndex];
            TElement right = _arrayFromPool[rightIndex];
            _arrayFromPool[leftIndex] = right;
            _arrayFromPool[rightIndex] = left;
            _indexMapPolicy.AddOrUpdate(_indexByElement, left, rightIndex);
            _indexMapPolicy.AddOrUpdate(_indexByElement, right, leftIndex);
        }

        private int Compare(TElement left, TElement right)
        {
            bool hasLeft = _priorityMapPolicy.TryGetValue(_priorityByElement, left, out TPriority leftPriority);
            bool hasRight = _priorityMapPolicy.TryGetValue(_priorityByElement, right, out TPriority rightPriority);
            if (!hasLeft)
                return hasRight ? 1 : 0;

            if (!hasRight)
                return -1;

            return _priorityComparer.Compare(leftPriority, rightPriority);
        }

        [Conditional("DEBUG")]
        private void VerifyHeap()
        {
            for (int i = 1; i < _arrayFromPool.Length; ++i)
            {
                int order = Compare(_arrayFromPool[i], _arrayFromPool[GetParentIndex(i)]);
                if (order < 0)
                    throw new InvalidOperationException("Element is smaller than it's parent.");
            }
        }

        private void HeapifyUp(int index)
        {
            throw new NotImplementedException();
        }

        private void HeapifyDown()
        {
            throw new NotImplementedException();
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
