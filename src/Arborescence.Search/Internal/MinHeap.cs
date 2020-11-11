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

        internal int Count => _count;

        public void Dispose()
        {
            TElement[] arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool != null)
                Pool.Return(arrayFromPool, ShouldClear());
        }

        internal void Add(TElement element)
        {
            EnsureCapacity();
            UncheckedAdd(element);
            VerifyHeap();
        }

        internal bool TryPeek(out TElement element)
        {
            if (_count == 0)
            {
                element = default;
                return false;
            }

            Debug.Assert(_arrayFromPool.Length > 0, "_arrayFromPool.Length > 0");

            element = _arrayFromPool[0];
            return true;
        }

        internal bool TryTake()
        {
            int count = _count;

            if (count == 0)
                return false;

            TElement[] array = _arrayFromPool;
            Debug.Assert(array.Length > 0, "array.Length > 0");

            TElement root = array[0];
            _indexMapPolicy.AddOrUpdate(_indexByElement, root, -1);
            if (count == 1)
            {
                _count = 0;
                if (ShouldClear())
                    array[0] = default;

                return true;
            }

            int newCount = count - 1;
            array[0] = array[newCount];
            _indexMapPolicy.AddOrUpdate(_indexByElement, array[0], 0);
            _count = newCount;
            if (ShouldClear())
                array[newCount] = default;

            HeapifyDown();
            VerifyHeap();
            return true;
        }

        // This function is also known as DecreaseKey.
        // It assumes the priority has already been updated (using an external write to the priority map or such).
        internal bool Update(TElement element)
        {
            bool hasIndex = _indexMapPolicy.TryGetValue(_indexByElement, element, out int index) && index != -1;
            if (hasIndex)
            {
                HeapifyUp(index);
                VerifyHeap();
            }

            return hasIndex;
        }

        internal bool Contains(TElement element) =>
            _indexMapPolicy.TryGetValue(_indexByElement, element, out int index) && index != -1;

        internal void AddOrUpdate(TElement element)
        {
            bool hasIndex = _indexMapPolicy.TryGetValue(_indexByElement, element, out int index) && index != -1;
            if (hasIndex)
            {
                HeapifyUp(index);
            }
            else
            {
                EnsureCapacity();
                UncheckedAdd(element);
            }

            VerifyHeap();
        }

        private void UncheckedAdd(TElement element)
        {
            int count = _count;
            TElement[] array = _arrayFromPool;
            Debug.Assert(unchecked((uint)count < (uint)array.Length), "(uint)count < (uint)array.Length");

            array[count] = element;
            _count = count + 1;
            _indexMapPolicy.AddOrUpdate(_indexByElement, element, count);
            HeapifyUp(count);
        }

        private void UncheckedGrow()
        {
            int count = _count;
            TElement[] arrayFromPool = _arrayFromPool;
            Debug.Assert(
                unchecked((uint)count == (uint)arrayFromPool.Length), "(uint)count == (uint)arrayFromPool.Length");

            int newCapacity = count > 0 ? count << 1 : DefaultCapacity;
            TElement[] newArrayFromPool = Pool.Rent(newCapacity);
            if (count > 0)
                Array.Copy(arrayFromPool, newArrayFromPool, count);

            _arrayFromPool = newArrayFromPool;
            Pool.Return(arrayFromPool, ShouldClear());
        }

        private void EnsureCapacity()
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert(unchecked((uint)count <= (uint)array.Length), "(uint)count <= (uint)array.Length");

            if (unchecked((uint)count == (uint)array.Length))
                UncheckedGrow();
        }

        private static int GetParent(int index) => (index - 1) / Arity;

        private static int GetChild(int index, int childIndex) => index * Arity + childIndex + 1;

        private void Swap(int leftIndex, int rightIndex)
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert(unchecked((uint)count <= (uint)array.Length), "(uint)count <= (uint)array.Length");
            Debug.Assert(unchecked((uint)leftIndex < (uint)count), "(uint)leftIndex < (uint)count");
            Debug.Assert(unchecked((uint)rightIndex < (uint)count), "(uint)rightIndex < (uint)count");

            TElement left = array[leftIndex];
            TElement right = array[rightIndex];
            array[leftIndex] = right;
            array[rightIndex] = left;
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
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert(unchecked((uint)count <= (uint)array.Length), "(uint)count <= (uint)array.Length");

            for (int i = 1; i < count; ++i)
            {
                int order = Compare(array[i], array[GetParent(i)]);
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
