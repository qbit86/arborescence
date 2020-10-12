namespace Arborescence.Workbench
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class IndexedEdgeListParser
    {
        private static readonly string[] s_arrowSeparator = { "->" };

        // Treats nodes as Base32 numbers: https://en.wikipedia.org/wiki/Base32#RFC_4648_Base32_alphabet

        public static IEnumerable<Endpoints> ParseEdges(TextReader textReader)
        {
            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            return ParseEdgesCore(textReader);
        }

        private static IEnumerable<Endpoints> ParseEdgesCore(TextReader textReader)
        {
            for (string? line = textReader.ReadLine(); line != null; line = textReader.ReadLine())
            {
                string[] parts = line.Split(s_arrowSeparator, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    continue;

                string[] leftTokens = parts[0].Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
                if (leftTokens.Length < 1)
                    continue;

                string leftToken = leftTokens[leftTokens.Length - 1];
                if (!TryParse(leftToken.AsSpan(), out int tail))
                    continue;

                string[] rightTokens = parts[1].Split(default(char[]), 2, StringSplitOptions.RemoveEmptyEntries);
                if (rightTokens.Length < 1)
                    continue;

                string rightToken = rightTokens[0];
                if (!TryParse(rightToken.AsSpan(), out int head))
                    continue;

                yield return new Endpoints(tail, head);
            }
        }

        private static bool TryParse(ReadOnlySpan<char> s, out int result) => Base32.TryParse(s, out result);
    }
}
