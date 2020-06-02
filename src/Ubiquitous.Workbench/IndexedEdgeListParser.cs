namespace Ubiquitous.Workbench
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using static System.Diagnostics.Debug;

    public static class IndexedEdgeListParser
    {
        private static readonly string[] s_arrowSeparator = { "->" };

        // Treats characters from ['a', 'z'] as integers from [0, 26).

        public static IEnumerable<SourceTargetPair<int>> ParseEdges(TextReader textReader)
        {
            if (textReader == null)
                throw new ArgumentNullException(nameof(textReader));

            return ParseEdgesCore(textReader);
        }

        private static IEnumerable<SourceTargetPair<int>> ParseEdgesCore(TextReader textReader)
        {
            Assert(textReader != null);

            for (string line = textReader.ReadLine(); line != null; line = textReader.ReadLine())
            {
                string[] parts = line.Split(s_arrowSeparator, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    continue;

                string[] leftTokens = parts[0].Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
                if (leftTokens.Length < 1)
                    continue;

                string leftToken = leftTokens[leftTokens.Length - 1];
                if (!int.TryParse(leftToken, NumberStyles.None, CultureInfo.InvariantCulture, out int source))
                {
                    if (!TryParse(leftToken.AsSpan(), out source))
                        continue;
                }

                string[] rightTokens = parts[1].Split(default(char[]), 2, StringSplitOptions.RemoveEmptyEntries);
                if (rightTokens.Length < 1)
                    continue;

                string rightToken = rightTokens[0];
                if (!int.TryParse(rightToken, NumberStyles.None, CultureInfo.InvariantCulture, out int target))
                {
                    if (!TryParse(rightToken.AsSpan(), out target))
                        continue;
                }

                yield return SourceTargetPair.Create(source, target);
            }
        }

        private static bool TryParse(ReadOnlySpan<char> s, out int result) => Base32.TryParse(s, out result);
    }
}
