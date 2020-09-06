namespace Arborescence.Traversal
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Defines methods to support getting and putting items for the map represented as a byte array.
    /// </summary>
    public readonly struct IndexedColorMapPolicy : IMapPolicy<byte[], int, Color>
    {
        /// <inheritdoc/>
        public bool TryGetValue(byte[] map, int key, out Color value)
        {
            if (map is null || (uint)key >= (uint)map.Length)
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
            if (map is null || (uint)key >= (uint)map.Length)
                return;

            map[key] = (byte)value;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
