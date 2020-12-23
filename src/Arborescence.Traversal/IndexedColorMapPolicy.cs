namespace Arborescence.Traversal
{
    using System;

    /// <summary>
    /// Defines methods to support getting and putting items for the map represented as a byte array.
    /// </summary>
    public readonly struct IndexedColorMapPolicy : IMapPolicy<byte[], int, Color>
    {
        /// <inheritdoc/>
        public bool TryGetValue(byte[] map, int key, out Color value)
        {
            if (map is null || unchecked((uint)key >= (uint)map.Length))
            {
                value = default;
                return false;
            }

            value = (Color)map[key];
            return true;
        }

        /// <inheritdoc/>
        public void AddOrUpdate(byte[] map, int key, Color value)
        {
            if (map is null)
                throw new ArgumentNullException(nameof(map));

            if (unchecked((uint)key >= (uint)map.Length))
                throw new ArgumentOutOfRangeException(nameof(key));

            map[key] = (byte)value;
        }
    }
}
