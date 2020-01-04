namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedMapPolicy<T> : IMapPolicy<T[], int, T>, IFactory<T[]>
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

        public bool TryGet(T[] map, int key, out T value)
        {
            if (map == null || (uint)key >= (uint)map.Length)
            {
                value = default;
                return false;
            }

            value = map[key];
            return true;
        }

        public bool TryPut(T[] map, int key, T value)
        {
            if (map == null || (uint)key >= (uint)map.Length)
                return false;

            map[key] = value;
            return true;
        }

        public T[] Acquire()
        {
            T[] array = Pool.Rent(Count);
            Array.Clear(array, 0, array.Length);
            return array;
        }

        public void Release(T[] value)
        {
            Pool.Return(value, true);
        }

        public void Warmup()
        {
            T[] array = Pool.Rent(Count);
            Pool.Return(array, true);
        }
    }
}
