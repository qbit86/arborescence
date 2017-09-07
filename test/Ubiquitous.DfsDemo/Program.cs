namespace Ubiquitous
{
    using System;

    internal static class Program
    {
        static void Main(string[] args)
        {
            const int vertexCount = 10;
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, 1.618));
            var builder = new IndexedAdjacencyListGraphBuilder(vertexCount);
            var prng = new Random(1729);
            for (int e = 0; e != edgeCount; ++e)
            {
                int source = prng.Next(vertexCount);
                int target = prng.Next(vertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }
            IndexedAdjacencyListGraph graph = builder.ToIndexedAdjacencyListGraph();

            Console.WriteLine($"{nameof(vertexCount)}: {vertexCount}, {nameof(edgeCount)}: {edgeCount}");
        }
    }
}
