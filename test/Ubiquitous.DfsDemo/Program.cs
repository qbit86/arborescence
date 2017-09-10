namespace Ubiquitous
{
    using System;
    using System.IO;
    using static System.Diagnostics.Debug;

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
        }

        private static void SerializeGraphByEdges(IndexedAdjacencyListGraph graph, string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph {graphName}{Environment.NewLine}{{");
            try
            {
                // TODO:
            }
            finally
            {
                textWriter.WriteLine("}");
            }
        }
    }
}
