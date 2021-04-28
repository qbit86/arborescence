namespace Arborescence.Internal
{
    using System;

    internal readonly struct IndexedMapPolicy<T> : IMapPolicy<T[], int, T>
    {
        public bool TryGetValue(T[] map, int key, out T value)
        {
            if (map is null || unchecked((uint)key >= (uint)map.Length))
            {
                value = default;
                return false;
            }

            value = map[key];
            return true;
        }

        public void AddOrUpdate(T[] map, int key, T value)
        {
            if (map is null)
                throw new ArgumentNullException(nameof(map));

            if ((uint)key >= (uint)map.Length)
                throw new ArgumentOutOfRangeException(nameof(key));

            map[key] = value;
        }
    }
}
