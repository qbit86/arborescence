namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedMapPolicy<T> : IMapPolicy<ArrayPrefix<T>, int, T>, IFactory<ArrayPrefix<T>>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public IndexedMapPolicy(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public int Count { get; }

        public bool TryGet(ArrayPrefix<T> map, int key, out T value)
        {
            if ((uint)key >= (uint)map.Count || map.Array == null)
            {
                value = default;
                return false;
            }

            value = map.Array[key];
            return true;
        }

        public bool TryPut(ArrayPrefix<T> map, int key, T value)
        {
            if ((uint)key >= (uint)map.Count || map.Array == null)
                return false;

            map.Array[key] = value;
            return true;
        }

        public ArrayPrefix<T> Acquire()
        {
            T[] array = Pool.Rent(Count);
            Array.Clear(array, 0, Count);
            return new ArrayPrefix<T>(array, Count);
        }

        public void Release(ArrayPrefix<T> value)
        {
            Pool.Return(value.Array, true);
        }

        public void Warmup()
        {
            T[] array = Pool.Rent(Count);
            Pool.Return(array, true);
        }
    }
}
