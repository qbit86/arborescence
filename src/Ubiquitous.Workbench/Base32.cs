namespace Ubiquitous.Workbench
{
    using System;

    // https://en.wikipedia.org/wiki/Base32#RFC_4648_Base32_alphabet

    public static class Base32
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz234567";

        public static string ToString(int value)
        {
            if (value == 0)
                return "a";

            uint remainingBits = (uint)value;
            Span<char> buffer = stackalloc char[7];
            int length = 0;
            for (int i = 0; i != buffer.Length; ++i)
            {
                uint mod = remainingBits & 0b11111;
                buffer[i] = Alphabet[(int)mod];
                remainingBits >>= 5;
                if (remainingBits == 0)
                {
                    length = i + 1;
                    break;
                }
            }

            Span<char> span = buffer.Slice(0, length);
            span.Reverse();
            return span.ToString();
        }
    }
}
