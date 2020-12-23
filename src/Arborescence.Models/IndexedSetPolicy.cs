namespace Arborescence.Models
{
    using System;

    /// <summary>
    /// Defines methods to support adding and checking items for the set represented as a byte array.
    /// </summary>
    public readonly struct IndexedSetPolicy : ISetPolicy<byte[], int>
    {
        /// <inheritdoc/>
        public bool Contains(byte[] items, int item)
        {
            if (items is null || unchecked((uint)item >= (uint)items.Length))
                return false;

            return items[item] != 0;
        }

        /// <inheritdoc/>
        public void Add(byte[] items, int item)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if ((uint)item >= (uint)items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            items[item] = 1;
        }
    }
}
