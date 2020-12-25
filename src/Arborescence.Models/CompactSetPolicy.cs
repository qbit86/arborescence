namespace Arborescence.Models
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines methods to support adding and checking items for the set represented as a compact array of bit values.
    /// </summary>
    public readonly struct CompactSetPolicy : ISetPolicy<byte[], int>
    {
        private const int BitShiftPerByte = 3;

        /// <summary>
        /// Get the number of bytes required to hold <paramref name="count"/> bit values.
        /// </summary>
        /// <param name="count">The number of bit values.</param>
        /// <returns>The number of bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetByteCount(int count)
        {
            if (count <= 0)
                return 0;

            uint temp = (uint)(count - 1 + (1 << BitShiftPerByte));
            return (int)(temp >> BitShiftPerByte);
        }

        /// <inheritdoc/>
        public bool Contains(byte[] items, int item)
        {
            int byteIndex = Div8Rem(item, out int bitIndex);
            if (items is null || unchecked((uint)byteIndex >= (uint)items.Length))
                return false;

            byte bitMask = (byte)(1 << bitIndex);

            return (items[byteIndex] & bitMask) != 0;
        }

        /// <inheritdoc/>
        public void Add(byte[] items, int item)
        {
            int byteIndex = Div8Rem(item, out int bitIndex);
            if (items is null || unchecked((uint)byteIndex >= (uint)items.Length))
                return;

            byte bitMask = (byte)(1 << bitIndex);

            items[byteIndex] |= bitMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Div8Rem(int number, out int remainder)
        {
            uint quotient = (uint)number >> 3;
            remainder = number & 0b111;
            return (int)quotient;
        }
    }
}
