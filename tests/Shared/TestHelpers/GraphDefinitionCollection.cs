namespace Arborescence;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Workbench;
using static System.FormattableString;

internal sealed class GraphDefinitionCollection : IEnumerable<object[]>
{
    private const int LowerBound = 1;
    private const int UpperBound = 13;

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = LowerBound; i < UpperBound; ++i)
        {
            string testCase = Invariant($"{i:D2}");

            using TextReader textReader = IndexedGraphs.GetTextReader(testCase);
            if (textReader == TextReader.Null)
                continue;

            var edges = Base32EdgeListParser.ParseEdges(textReader).ToList();
            int vertexCount = edges.Count == 0 ? 0 : edges.Select(e => Math.Max(e.Tail, e.Head)).Max() + 1;
            string description = $"{{{nameof(testCase)}: {testCase}}}";
            GraphDefinitionParameter parameter = new(vertexCount, edges, description);
            yield return new object[] { parameter };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
