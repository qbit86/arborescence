namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Workbench;

#pragma warning disable CA1812 // GraphDefinitionCollection is an internal class that is apparently never instantiated.
    internal sealed class GraphDefinitionCollection : IEnumerable<object[]>
    {
        private const int LowerBound = 1;
        private const int UpperBound = 10;

        private static readonly double[] s_densityPowers = { 1.0, 1.5, 2.0 };

        private static CultureInfo F => CultureInfo.InvariantCulture;

        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = LowerBound; i < UpperBound; ++i)
            {
                string testCase = i.ToString("D2", CultureInfo.InvariantCulture);

                using TextReader textReader = IndexedGraphs.GetTextReader(testCase);
                if (textReader == TextReader.Null)
                    continue;

                var edges = IndexedEdgeListParser.ParseEdges(textReader).ToList();
                int vertexCount = edges.Count == 0 ? 0 : edges.Select(e => Math.Max(e.Tail, e.Head)).Max() + 1;
                string description = $"{{{nameof(testCase)}: {testCase}}}";
                var parameter = new GraphDefinitionParameter(vertexCount, edges, description);
                yield return new object[] { parameter };
            }

            for (int i = 1; i < 7; ++i)
            {
                double power = 0.5 * i;
                int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
                foreach (double densityPower in s_densityPowers)
                {
                    var edges = new List<Endpoints>();
                    GraphHelper.GenerateEdges(vertexCount, densityPower, edges);
                    string description =
                        $"{{{nameof(vertexCount)}: {vertexCount.ToString(F)}, {nameof(densityPower)}: {densityPower.ToString(F)}}}";
                    var parameter = new GraphDefinitionParameter(vertexCount, edges, description);
                    yield return new object[] { parameter };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CA1812 // GraphDefinitionCollection is an internal class that is apparently never instantiated.
}
