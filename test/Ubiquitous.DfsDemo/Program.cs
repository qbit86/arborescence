namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using static System.Diagnostics.Debug;

    internal struct ColorMapFactoryInstance : IFactoryConcept<IndexedAdjacencyListGraph, IndexedDictionary<Color, Color[]>>
    {
        public IndexedDictionary<Color, Color[]> Acquire(IndexedAdjacencyListGraph graph)
        {
            return IndexedDictionary.Create(new Color[graph.VertexCount]);
        }

        public void Release(IndexedAdjacencyListGraph graph, IndexedDictionary<Color, Color[]> value)
        {
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            const int vertexCount = 9;
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.618));

            Console.WriteLine($"{nameof(vertexCount)}: {vertexCount}, {nameof(edgeCount)}: {edgeCount}");

            var builder = new IndexedAdjacencyListGraphBuilder(vertexCount);
            var prng = new Random(1729);

            // Making sure that each vertex has at least one nontrivial out-edge.
            for (int v = 0; v < vertexCount; ++v)
            {
                int source = v;
                int target = (v + 1 + prng.Next(vertexCount - 1)) % vertexCount;
                builder.Add(SourceTargetPair.Create(source, target));
            }

            // Adding the rest of vertices.
            for (int e = vertexCount; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexCount);
                int target = prng.Next(vertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }

            IndexedAdjacencyListGraph graph = builder.ToIndexedAdjacencyListGraph();

            var dfs = new Dfs<IndexedAdjacencyListGraph, int, int, IEnumerable<int>, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance>();
            var steps = dfs.TraverseRecursively<IndexedDictionary<Color, Color[]>, ColorMapFactoryInstance>(graph, 0);

            var edgeKinds = IndexedDictionary.Create(new DfsStepKind[graph.EdgeCount]);
            foreach (var step in steps)
            {
                switch (step.Kind)
                {
                    case DfsStepKind.TreeEdge:
                    case DfsStepKind.BackEdge:
                    case DfsStepKind.ForwardOrCrossEdge:
                        edgeKinds[step.Edge] = step.Kind;
                        break;
                    default:
                        continue;
                }
            }

            SerializeGraphByEdges(graph, edgeKinds, "DFS", Console.Out);
        }

        private static void SerializeGraphByEdges(IndexedAdjacencyListGraph graph, IReadOnlyDictionary<int, DfsStepKind> edgeKinds, string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\"{Environment.NewLine}{{");
            textWriter.WriteLine($"    node [shape=circle]");
            try
            {
                for (int e = 0; e < graph.EdgeCount; ++e)
                {
                    SourceTargetPair<int> endpoints;
                    if (graph.TryGetEndpoints(e, out endpoints))
                    {
                        textWriter.Write($"    {endpoints.Source} -> {endpoints.Target}");

                        DfsStepKind edgeKind;
                        if (edgeKinds == null || !edgeKinds.TryGetValue(e, out edgeKind))
                        {
                            textWriter.WriteLine();
                            continue;
                        }

                        // http://www.graphviz.org/Documentation/dotguide.pdf
                        switch (edgeKind)
                        {
                            case DfsStepKind.TreeEdge:
                                textWriter.WriteLine($" [style=bold]");
                                continue;
                            case DfsStepKind.BackEdge:
                                textWriter.WriteLine($" [style=dashed]");
                                continue;
                            case DfsStepKind.ForwardOrCrossEdge:
                                textWriter.WriteLine($" [style=solid]");
                                continue;
                        }

                        textWriter.WriteLine($" [style=dotted]");
                    }
                }
            }
            finally
            {
                textWriter.WriteLine("}");
            }
        }
    }
}
