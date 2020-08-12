namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Workbench;

#pragma warning disable CA1812 // GraphCollection is an internal class that is apparently never instantiated.
    internal sealed class GraphCollection : IEnumerable<object[]>
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
                var builder = new IndexedIncidenceGraph.Builder(10);

                using (TextReader textReader = IndexedGraphs.GetTextReader(testCase))
                {
                    if (textReader is null || textReader == TextReader.Null)
                        continue;

                    IEnumerable<Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
                    foreach (Endpoints edge in edges)
                        builder.TryAdd(edge.Tail, edge.Head, out _);
                }

                IndexedIncidenceGraph graph = builder.ToGraph();
                string description = $"{{{nameof(testCase)}: {testCase}}}";
                var graphParameter = GraphParameter.Create(graph, description);
                yield return new object[] { graphParameter };
            }

            {
                const int vertexCount = 1;
                const double densityPower = 1.0;
                var builder = new IndexedIncidenceGraph.Builder(1);
                IndexedIncidenceGraph graph = GraphHelper.GenerateIncidenceGraph<
                    IndexedIncidenceGraph, int, ArraySegmentEnumerator<int>, IndexedIncidenceGraph.Builder>(
                    builder, vertexCount, densityPower);
                string description =
                    $"{{{nameof(vertexCount)}: {vertexCount.ToString(F)}, {nameof(densityPower)}: {densityPower.ToString(F)}}}";
                var graphParameter = GraphParameter.Create(graph, description);
                yield return new object[] { graphParameter };
            }

            for (int i = 1; i < 7; ++i)
            {
                double power = 0.5 * i;
                int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
                foreach (double densityPower in s_densityPowers)
                {
                    var builder = new IndexedIncidenceGraph.Builder(1);
                    IndexedIncidenceGraph graph = GraphHelper.GenerateIncidenceGraph<
                        IndexedIncidenceGraph, int, ArraySegmentEnumerator<int>, IndexedIncidenceGraph.Builder>(
                        builder, vertexCount, densityPower);
                    string description =
                        $"{{{nameof(vertexCount)}: {vertexCount.ToString(F)}, {nameof(densityPower)}: {densityPower.ToString(F)}}}";
                    var graphParameter = GraphParameter.Create(graph, description);
                    yield return new object[] { graphParameter };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CA1812 // GraphCollection is an internal class that is apparently never instantiated.
}
