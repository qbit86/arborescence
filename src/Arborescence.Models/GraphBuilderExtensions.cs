namespace Arborescence.Models
{
    using System;

    public static class GraphBuilderExtensions
    {
        public static bool TryAdd<TGraph, TVertex, TEdge>(
            this IGraphBuilder<TGraph, TVertex, TEdge> graphBuilder, TVertex tail, TVertex head)
        {
            if (graphBuilder is null)
                throw new ArgumentNullException(nameof(graphBuilder));

            return graphBuilder.TryAdd(tail, head, out TEdge _);
        }
    }
}
