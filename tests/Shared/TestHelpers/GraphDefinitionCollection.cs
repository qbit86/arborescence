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

    private static readonly double[] s_densityPowers = { 1.0, 1.5, 2.0 };

    private static CultureInfo P => CultureInfo.InvariantCulture;

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = LowerBound; i < UpperBound; ++i)
        {
            string testCase = i.ToString("D2", CultureInfo.InvariantCulture);

            using TextReader textReader = IndexedGraphs.GetTextReader(testCase);
            if (textReader == TextReader.Null)
                continue;

            var edges = IndexedEdgeListParser.ParseEdges(textReader).Select(Transform).ToList();
            int vertexCount = edges.Count == 0 ? 0 : edges.Select(e => Math.Max(e.Tail, e.Head)).Max() + 1;
            string description = $"{{{nameof(testCase)}: {testCase}}}";
            GraphDefinitionParameter parameter = new(vertexCount, edges, description);
            yield return new object[] { parameter };
        }

        for (int i = 1; i < 6; ++i)
        {
            double power = 0.5 * i;
            int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
            foreach (double densityPower in s_densityPowers)
            {
                List<Endpoints<int>> edges = new();
                GraphHelpers.GenerateEdges(vertexCount, densityPower, edges);
                string description =
                    $"{{{nameof(vertexCount)}: {vertexCount.ToString(P)}, {nameof(densityPower)}: {densityPower.ToString(P)}}}";
                GraphDefinitionParameter parameter = new(vertexCount, edges, description);
                yield return new object[] { parameter };
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static Endpoints<int> Transform(Int32Endpoints endpoints) => new(endpoints.Tail, endpoints.Head);
}
