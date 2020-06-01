namespace Ubiquitous.Workbench
{
    using System;

    // https://en.wikipedia.org/wiki/Base32#RFC_4648_Base32_alphabet

    public static class Base32
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz234567";
        private const int MaxLength = 7;

        public static string ToString(int value)
        {
            if (value == 0)
                return "a";

            uint remainingBits = (uint)value;
            Span<char> buffer = stackalloc char[MaxLength];
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

        public static bool TryParse(ReadOnlySpan<char> value, out int result)
        {
            ReadOnlySpan<char> input = value.Length > MaxLength
                ? value.Slice(value.Length - MaxLength, MaxLength)
                : value;
            Span<byte> buffer = stackalloc byte[MaxLength];

            for (int i = 0; i != input.Length; ++i)
            {
                char c = char.ToLowerInvariant(input[i]);
                if (c >= 'a' && c <= 'z')
                {
                    buffer[i] = (byte)(c - 'a');
                    continue;
                }

                if (c >= '2' && c <= '7')
                {
                    buffer[i] = (byte)(26 + c - '2');
                    continue;
                }

                result = default;
                return false;
            }

            throw new NotImplementedException();
        }
    }
}
