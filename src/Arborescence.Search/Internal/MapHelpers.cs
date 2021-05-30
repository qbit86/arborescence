namespace Arborescence.Search
{
    using System;

    internal static class MapHelpers
    {
        internal static bool ContainsKey(byte[] items, int key)
        {
            if (unchecked((uint)key >= items.Length))
                return false;

            return items[key] != 0;
        }

        internal static bool TryGetValue(byte[] items, int key, out byte value)
        {
            if (unchecked((uint)key >= items.Length))
            {
                value = default;
                return false;
            }

            value = items[key];
            return true;
        }

        internal static void AddOrUpdate(byte[] items, int key, byte value)
        {
            if ((uint)key >= items.Length)
                throw new ArgumentOutOfRangeException(nameof(key));

            items[key] = value;
        }
    }
}
