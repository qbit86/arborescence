namespace Arborescence;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Workbench;

internal sealed class GraphDefinitionCollection : IEnumerable<object[]>
{
    private const int LowerBound = 1;
    private const int UpperBound = 10;

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = LowerBound; i < UpperBound; ++i)
        {
            string testCase = i.ToString("D2", CultureInfo.InvariantCulture);

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
