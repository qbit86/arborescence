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
            const int vertexCount = 10;
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
            SerializeGraphByEdges(graph, null, "Graph", Console.Out);

            var dfs = new Dfs<IndexedAdjacencyListGraph, int, int, IEnumerable<int>, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance>();
            var steps = dfs.TraverseRecursively<IndexedDictionary<Color, Color[]>, ColorMapFactoryInstance>(graph, 0);

            var edgeKinds = IndexedDictionary.Create(new StepKind[graph.EdgeCount]);
            foreach (var step in steps)
            {
                switch (step.Kind)
                {
                    case StepKind.TreeEdge:
                    case StepKind.BackEdge:
                    case StepKind.ForwardOrCrossEdge:
                    case StepKind.NonTreeEdge:
                        edgeKinds[step.Edge] = step.Kind;
                        break;
                    default:
                        continue;
                }
            }

            SerializeGraphByEdges(graph, edgeKinds, "DFS", Console.Out);
        }

        private static void SerializeGraphByEdges(IndexedAdjacencyListGraph graph, IReadOnlyDictionary<int, StepKind> edgeKinds, string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\"{Environment.NewLine}{{");
            try
            {
                for (int e = 0; e < graph.EdgeCount; ++e)
                {
                    SourceTargetPair<int> endpoints;
                    if (graph.TryGetEndpoints(e, out endpoints))
                    {
                        textWriter.Write($"    {endpoints.Source} -> {endpoints.Target}");

                        StepKind edgeKind;
                        if (edgeKinds == null || !edgeKinds.TryGetValue(e, out edgeKind))
                        {
                            textWriter.WriteLine();
                            continue;
                        }

                        switch (edgeKind)
                        {
                            case StepKind.TreeEdge:
                                textWriter.WriteLine($" [penwidth = 3]");
                                continue;
                        }

                        textWriter.WriteLine();
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
