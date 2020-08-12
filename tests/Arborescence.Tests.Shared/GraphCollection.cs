namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Workbench;

    internal abstract class GraphCollection<TGraph, TEdge, TEdges, TGraphBuilder> : IEnumerable<object[]>
        where TGraph : IIncidenceGraph<int, TEdge, TEdges>
        where TGraphBuilder : IGraphBuilder<TGraph, int, TEdge>
    {
        private const int LowerBound = 1;
        private const int UpperBound = 10;

        private static CultureInfo F => CultureInfo.InvariantCulture;

        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = LowerBound; i < UpperBound; ++i)
            {
                string testCase = i.ToString("D2", CultureInfo.InvariantCulture);
                TGraphBuilder builder = CreateGraphBuilder(0);

                using (TextReader textReader = IndexedGraphs.GetTextReader(testCase))
                {
                    if (textReader is null || textReader == TextReader.Null)
                        continue;

                    IEnumerable<Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
                    foreach (Endpoints edge in edges)
                        builder.TryAdd(edge.Tail, edge.Head, out _);
                }

                TGraph graph = builder.ToGraph();
                string description = $"{{{nameof(testCase)}: {testCase}}}";
                var graphParameter = GraphParameter.Create(graph, description);
                yield return new object[] { graphParameter };
            }

            {
                const int vertexCount = 1;
                const double densityPower = 1.0;
                TGraphBuilder builder = CreateGraphBuilder(1);
                TGraph graph = GraphHelper.GenerateIncidenceGraph<TGraph, TEdge, TEdges, TGraphBuilder>(
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
                foreach (double densityPower in GraphHelper.DensityPowers)
                {
                    TGraphBuilder builder = CreateGraphBuilder(1);
                    TGraph graph = GraphHelper.GenerateIncidenceGraph<TGraph, TEdge, TEdges, TGraphBuilder>(
                        builder, vertexCount, densityPower);
                    string description =
                        $"{{{nameof(vertexCount)}: {vertexCount.ToString(F)}, {nameof(densityPower)}: {densityPower.ToString(F)}}}";
                    var graphParameter = GraphParameter.Create(graph, description);
                    yield return new object[] { graphParameter };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected abstract TGraphBuilder CreateGraphBuilder(int initialVertexCount);
    }

#pragma warning disable CA1812 // GraphCollection is an internal class that is apparently never instantiated.
    internal sealed class IndexedGraphCollection :
        GraphCollection<IndexedIncidenceGraph, int, ArraySegmentEnumerator<int>, IndexedIncidenceGraph.Builder>
    {
        protected override IndexedIncidenceGraph.Builder CreateGraphBuilder(int initialVertexCount)
        {
            return new IndexedIncidenceGraph.Builder(initialVertexCount);
        }
    }

    internal sealed class FromMutableIndexedGraphCollection :
        GraphCollection<IndexedIncidenceGraph, int, ArraySegmentEnumerator<int>, MutableIndexedIncidenceGraph>
    {
        protected override MutableIndexedIncidenceGraph CreateGraphBuilder(int initialVertexCount)
        {
            return new MutableIndexedIncidenceGraph(initialVertexCount);
        }
    }
#pragma warning restore CA1812 // GraphCollection is an internal class that is apparently never instantiated.
}
