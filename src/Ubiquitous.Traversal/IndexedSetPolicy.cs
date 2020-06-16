namespace Ubiquitous.Traversal
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedSetPolicy : ISetPolicy<byte[], int>
    {
        public bool Contains(byte[] items, int item)
        {
            if (items == null || (uint)item >= (uint)items.Length)
                return false;

            return items[item] != 0;
        }

        public void Add(byte[] items, int item)
        {
            if (items == null || (uint)item >= (uint)items.Length)
                return;

            items[item] = 0x01;
        }

        public void Clear(byte[] items)
        {
            if (items == null)
                return;

            Array.Clear(items, 0, items.Length);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
