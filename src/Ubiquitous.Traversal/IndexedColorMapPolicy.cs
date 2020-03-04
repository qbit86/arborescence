namespace Ubiquitous.Traversal
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedColorMapPolicy : IMapPolicy<byte[], int, Color>
    {
        public bool TryGetValue(byte[] map, int key, out Color value)
        {
            if (map == null || (uint)key >= (uint)map.Length)
            {
                value = default;
                return false;
            }

            value = (Color)map[key];
            return true;
        }

        public void AddOrUpdate(byte[] map, int key, Color value)
        {
            if (map == null || (uint)key >= (uint)map.Length)
                return;

            map[key] = (byte)value;
        }

        public void Clear(byte[] map)
        {
            if (map == null)
                return;

            Array.Clear(map, 0, map.Length);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
