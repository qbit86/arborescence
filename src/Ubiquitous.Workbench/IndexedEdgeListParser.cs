namespace Ubiquitous.Workbench
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public readonly struct IndexedEdgeListParser
    {
        private static readonly string[] s_arrowSeparator = { "->" };

        // Treats characters from ['a', 'z'] as integers from [0, 26).

        public IEnumerable<SourceTargetPair<int>> ParseEdges(TextReader textReader)
        {
            if (textReader == null)
                throw new ArgumentNullException(nameof(textReader));

            return ParseEdgesCore();

            IEnumerable<SourceTargetPair<int>> ParseEdgesCore()
            {
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
                        if (leftToken.Length != 1)
                            continue;

                        char sourceChar = leftToken[0];
                        if (sourceChar < 'a' || sourceChar > 'z')
                            continue;

                        source = sourceChar - 'a';
                    }

                    string[] rightTokens = parts[1].Split(default(char[]), 2, StringSplitOptions.RemoveEmptyEntries);
                    if (rightTokens.Length < 1)
                        continue;

                    string rightToken = rightTokens[0];
                    if (!int.TryParse(rightToken, NumberStyles.None, CultureInfo.InvariantCulture, out int target))
                    {
                        if (rightToken.Length != 1)
                            continue;

                        char targetChar = rightToken[0];
                        if (targetChar < 'a' || targetChar > 'z')
                            continue;

                        target = targetChar - 'a';
                    }

                    yield return SourceTargetPair.Create(source, target);
                }
            }
        }
    }
}
