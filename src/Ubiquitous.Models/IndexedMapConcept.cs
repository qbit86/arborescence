namespace Ubiquitous
{
    using System;
    using System.Buffers;

    public readonly struct IndexedMapConcept<TGraph, T>
        : IMapConcept<ArraySegment<T>, int, T>, IFactory<TGraph, ArraySegment<T>>
    {
        public IndexedMapConcept(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public int Count { get; }

        public bool TryGet(ArraySegment<T> map, int key, out T value)
        {
            if ((uint)key >= (uint)map.Count || map.Array == null)
            {
                value = default;
                return false;
            }

            value = map.Array[key + map.Offset];
            return true;
        }

        public bool TryPut(ArraySegment<T> map, int key, T value)
        {
            if ((uint)key >= (uint)map.Count || map.Array == null)
                return false;

            map.Array[key + map.Offset] = value;
            return true;
        }

        public ArraySegment<T> Acquire(TGraph context)
        {
            T[] array = ArrayPool<T>.Shared.Rent(Count);
            Array.Clear(array, 0, Count);
            return new ArraySegment<T>(array, 0, Count);
        }

        public void Release(TGraph context, ArraySegment<T> value)
        {
            ArrayPool<T>.Shared.Return(value.Array, true);
        }

        public void Warmup()
        {
            T[] array = ArrayPool<T>.Shared.Rent(Count);
            ArrayPool<T>.Shared.Return(array, true);
        }
    }
}
