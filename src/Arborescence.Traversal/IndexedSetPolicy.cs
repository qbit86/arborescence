namespace Arborescence.Traversal
{
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

            items[item] = 1;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
