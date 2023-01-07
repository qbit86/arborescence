namespace Arborescence.Search
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/detail/d_ary_heap.hpp
    // https://github.com/dotnet/runtime/tree/main/src/libraries/System.Collections/src/System/Collections/Generic/PriorityQueue.cs

    internal struct MinHeap<TElement, TPriority, TPriorityMap, TIndexInHeapMap, TPriorityComparer> : IDisposable
        where TPriorityMap : IReadOnlyDictionary<TElement, TPriority>
        where TIndexInHeapMap : IDictionary<TElement, int>
        where TPriorityComparer : IComparer<TPriority>
        where TElement : notnull
    {
        private const int Arity = 4;
        private const int Log2Arity = 2;

        private TElement[] _arrayFromPool;
        private int _count;
        private readonly TPriorityMap _priorityByElement;
        private TIndexInHeapMap _indexInHeapByElement;
        private readonly TPriorityComparer _priorityComparer;

        internal MinHeap(
            TPriorityMap priorityByElement, TIndexInHeapMap indexInHeapByElement, TPriorityComparer priorityComparer)
        {
            if (priorityByElement is null)
                ThrowHelper.ThrowArgumentNullException(nameof(priorityByElement));

            if (indexInHeapByElement is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexInHeapByElement));

            if (priorityComparer is null)
                ThrowHelper.ThrowArgumentNullException(nameof(priorityComparer));

            _arrayFromPool = Array.Empty<TElement>();
            _count = 0;
            _priorityByElement = priorityByElement;
            _indexInHeapByElement = indexInHeapByElement;
            _priorityComparer = priorityComparer;
        }

        private static ArrayPool<TElement> Pool => ArrayPool<TElement>.Shared;

        internal readonly int Count => _count;

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

        internal readonly bool TryPeek([MaybeNullWhen(false)] out TElement element)
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

        internal bool TryTake([MaybeNullWhen(false)] out TElement element)
        {
            int count = _count;

            if (count == 0)
            {
                element = default;
                return false;
            }

            TElement[] array = _arrayFromPool;
            Debug.Assert(array.Length > 0, "array.Length > 0");

            element = array[0];
            _indexInHeapByElement[element] = -1;
            if (count == 1)
            {
                _count = 0;
                if (ShouldClear())
                    array[0] = default!;

                return true;
            }

            int newCount = count - 1;
            array[0] = array[newCount];
            _indexInHeapByElement[array[0]] = 0;
            _count = newCount;
            if (ShouldClear())
                array[newCount] = default!;

            HeapifyDown();
            VerifyHeap();
            return true;
        }

        // This function is also known as DecreaseKey.
        // It assumes the priority has already been updated (using an external write to the priority map or such).
        internal bool Update(TElement element)
        {
            bool hasIndex = _indexInHeapByElement.TryGetValue(element, out int index) && index != -1;
            if (hasIndex)
            {
                HeapifyUp(index);
                VerifyHeap();
            }

            return hasIndex;
        }

        internal readonly bool Contains(TElement element) =>
            _indexInHeapByElement.TryGetValue(element, out int index) && index != -1;

        internal void AddOrUpdate(TElement element)
        {
            bool hasIndex = _indexInHeapByElement.TryGetValue(element, out int index) && index != -1;
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

        private readonly TPriority GetPriorityOrThrow(TElement element)
        {
            if (_priorityByElement.TryGetValue(element, out TPriority? priority))
                return priority;

            throw new InvalidOperationException("Priority was not found for the given element.");
        }

        private void UncheckedAdd(TElement element)
        {
            int count = _count;
            TElement[] array = _arrayFromPool;
            Debug.Assert((uint)count < (uint)array.Length, "(uint)count < (uint)array.Length");

            array[count] = element;
            _count = count + 1;
            _indexInHeapByElement[element] = count;
            HeapifyUp(count);
        }

        private void UncheckedGrow()
        {
            int count = _count;
            TElement[] arrayFromPool = _arrayFromPool;
            Debug.Assert((uint)count == (uint)arrayFromPool.Length, "(uint)count == (uint)arrayFromPool.Length");

            const int defaultCapacity = 4;
            int newCapacity = count > 0 ? count << 1 : defaultCapacity;
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
            Debug.Assert((uint)count <= (uint)array.Length, "(uint)count <= (uint)array.Length");

            if (count == array.Length)
                UncheckedGrow();
        }

        private static int GetParentIndex(int index) => (index - 1) >> Log2Arity;

        private static int GetFirstChildIndex(int index) => (index << Log2Arity) + 1;

        private void Swap(int leftIndex, int rightIndex)
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert((uint)count <= (uint)array.Length, "(uint)count <= (uint)array.Length");
            Debug.Assert((uint)leftIndex < (uint)count, "(uint)leftIndex < (uint)count");
            Debug.Assert((uint)rightIndex < (uint)count, "(uint)rightIndex < (uint)count");

            TElement left = array[leftIndex];
            TElement right = array[rightIndex];
            array[leftIndex] = right;
            array[rightIndex] = left;
            _indexInHeapByElement[left] = rightIndex;
            _indexInHeapByElement[right] = leftIndex;
        }

        private readonly int Compare(TElement left, TElement right)
        {
            bool hasLeft = _priorityByElement.TryGetValue(left, out TPriority? leftPriority);
            bool hasRight = _priorityByElement.TryGetValue(right, out TPriority? rightPriority);
            if (!hasLeft)
                return hasRight ? 1 : 0;

            if (!hasRight)
                return -1;

            return _priorityComparer.Compare(leftPriority, rightPriority);
        }

        private void HeapifyUp(int index)
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert((uint)count <= (uint)array.Length, "(uint)count <= (uint)array.Length");

            if (index == 0 || count <= 1)
                return;

            if (index >= count)
                return;

            TElement currentlyBeingMovedElement = array[index];
            TPriority currentlyBeingMovedPriority = GetPriorityOrThrow(currentlyBeingMovedElement);

            int levelMovedCount = 0;
            for (int ascendingIndex = index; ascendingIndex != 0;)
            {
                int parentIndex = GetParentIndex(ascendingIndex);
                TElement parentElement = array[parentIndex];
                TPriority parentPriority = GetPriorityOrThrow(parentElement);
                if (_priorityComparer.Compare(currentlyBeingMovedPriority, parentPriority) < 0)
                {
                    ++levelMovedCount;
                    ascendingIndex = parentIndex;
                    continue;
                }

                break;
            }

            Debug.Assert(levelMovedCount > 0, "levelMovedCount > 0");

            int topIndex = index;
            for (int i = 0; i < levelMovedCount; ++i)
            {
                int parentIndex = GetParentIndex(topIndex);
                TElement parentElement = array[parentIndex];
                _indexInHeapByElement[parentElement] = topIndex;
                array[topIndex] = parentElement;
                topIndex = parentIndex;
            }

            array[topIndex] = currentlyBeingMovedElement;
            _indexInHeapByElement[currentlyBeingMovedElement] = topIndex;
        }

        private void HeapifyDown()
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert((uint)count <= (uint)array.Length, "(uint)count <= (uint)array.Length");

            if (count <= 1)
                return;

            int currentlyBeingMovedIndex = 0;
            TElement currentlyBeingMovedElement = array[currentlyBeingMovedIndex];
            TPriority currentlyBeingMovedPriority = GetPriorityOrThrow(currentlyBeingMovedElement);
            while (true)
            {
                int childrenOffset = GetFirstChildIndex(currentlyBeingMovedIndex);
                int childCount = Math.Min(count - childrenOffset, Arity);
                if (childCount <= 0)
                    break;

                var children = new Span<TElement>(array, childrenOffset, childCount);
                int smallestChildIndex = 0;
                TPriority smallestChildPriority = GetPriorityOrThrow(children[smallestChildIndex]);
                for (int i = 1; i < childCount; ++i)
                {
                    TElement child = children[i];
                    TPriority priority = GetPriorityOrThrow(child);
                    if (_priorityComparer.Compare(priority, smallestChildPriority) < 0)
                    {
                        smallestChildIndex = i;
                        smallestChildPriority = priority;
                    }
                }

                if (_priorityComparer.Compare(smallestChildPriority, currentlyBeingMovedPriority) < 0)
                {
                    Swap(childrenOffset + smallestChildIndex, currentlyBeingMovedIndex);
                    currentlyBeingMovedIndex = childrenOffset + smallestChildIndex;
                    continue;
                }

                break;
            }
        }

        [Conditional("DEBUG")]
        private readonly void VerifyHeap()
        {
            TElement[] array = _arrayFromPool;
            int count = _count;
            Debug.Assert((uint)count <= (uint)array.Length, "(uint)count <= (uint)array.Length");

            for (int i = 1; i < count; ++i)
            {
                int order = Compare(array[i], array[GetParentIndex(i)]);
                if (order < 0)
                    throw new InvalidOperationException("Element is smaller than its parent.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ShouldClear()
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
            return RuntimeHelpers.IsReferenceOrContainsReferences<TElement>();
#else
            return true;
#endif
        }
    }
}
