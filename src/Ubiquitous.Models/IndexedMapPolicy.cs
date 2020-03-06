namespace Ubiquitous.Models
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedMapPolicy<T> : IMapPolicy<T[], int, T>
    {
        public bool TryGetValue(T[] map, int key, out T value)
        {
            if (map == null || (uint)key >= (uint)map.Length)
            {
                value = default;
                return false;
            }

            value = map[key];
            return true;
        }

        public void AddOrUpdate(T[] map, int key, T value)
        {
            if (map == null || (uint)key >= (uint)map.Length)
                return;

            map[key] = value;
        }

        public void Clear(T[] map)
        {
            if (map == null)
                return;

            Array.Clear(map, 0, map.Length);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
