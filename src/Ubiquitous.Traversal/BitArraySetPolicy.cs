namespace Ubiquitous.Traversal
{
    using System.Collections;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct BitArraySetPolicy : ISetPolicy<BitArray, int>
    {
        public bool Contains(BitArray items, int item)
        {
            if (items == null || (uint)item >= (uint)items.Length)
                return false;

            return items.Get(item);
        }

        public void Add(BitArray items, int item)
        {
            if (items == null || (uint)item >= (uint)items.Length)
                return;

            items.Set(item, true);
        }

        public void Clear(BitArray items) => items?.SetAll(false);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
