namespace Arborescence
{
    using System;
    using Models;

    internal static class Program
    {
        private static void Main()
        {
            var builder = new SimpleIncidenceGraph.Builder();
            builder.TryAdd(0, 2);
            builder.TryAdd(0, 3);
            builder.TryAdd(2, 3);
            builder.TryAdd(3, 2);
            SimpleIncidenceGraph graph = builder.ToGraph();
            Console.WriteLine($"VertexCount: {graph.VertexCount}");
        }
    }
}
