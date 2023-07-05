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
            if ((uint)item >= (uint)items.Length)
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(item));

            items[item] = 1;
        }
    }
}
