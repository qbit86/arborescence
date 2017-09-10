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
            return new IndexedDictionary<Color, Color[]>(new Color[graph.VertexCount]);
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
            SerializeGraphByEdges(graph, "Graph", Console.Out);

            var dfs = new Dfs<IndexedAdjacencyListGraph, int, int, IEnumerable<int>, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance>();
            var steps = dfs.TraverseRecursively<IndexedDictionary<Color, Color[]>, ColorMapFactoryInstance>(graph, 0);

            var edgeKinds = new StepKind[graph.EdgeCount];
            foreach (var step in steps)
            {
                if (Step.IsEdge(step.Kind))
                    edgeKinds[step.Edge] = step.Kind;
            }

            // TODO: Serialize with edge kinds.
        }

        private static void SerializeGraphByEdges(IndexedAdjacencyListGraph graph, string graphName, TextWriter textWriter)
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
                        textWriter.WriteLine($"    {endpoints.Source} -> {endpoints.Target};");
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
