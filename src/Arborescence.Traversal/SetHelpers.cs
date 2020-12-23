namespace Arborescence.Traversal
{
    using System;

    internal static class SetHelpers
    {
        internal static bool Contains(byte[] items, int item)
        {
            if (unchecked((uint)item >= (uint)items.Length))
                return false;

            return items[item] != 0;
        }

        internal static void Add(byte[] items, int item)
        {
            if (unchecked((uint)item >= (uint)items.Length))
                throw new ArgumentOutOfRangeException(nameof(item));

            items[item] = 1;
        }
    }
}
