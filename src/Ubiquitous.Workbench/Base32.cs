namespace Ubiquitous.Workbench
{
    using System;

    public static class Base32
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz234567";

        public static string ToString(int value)
        {
            if (value == 0)
                return "a";

            uint l = (uint)value;
            Span<char> buffer = stackalloc char[7];
            int index = 0;
            for (int i = 0; i != buffer.Length; i++)
            {
                uint mod = l & 0b11111;
                buffer[i] = Alphabet[(int)mod];
                l >>= 5;
                if (l == 0)
                {
                    index = i + 1;
                    break;
                }
            }

            Span<char> span = buffer.Slice(0, index);
            span.Reverse();
            return span.ToString();
        }
    }
}
