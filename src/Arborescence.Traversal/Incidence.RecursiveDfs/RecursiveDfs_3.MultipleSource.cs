namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Threading;

    public static partial class RecursiveDfs<TVertex, TEdge, TEdgeEnumerator>
    {
        private static void TraverseUnchecked<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            using IEnumerator<TVertex> sourceEnumerator = sources.GetEnumerator();
            while (sourceEnumerator.MoveNext())
            {
                TVertex source = sourceEnumerator.Current;
                Color color = GetColorOrDefault(colorByVertex, source);
                if (color is not (Color.None or Color.White))
                    continue;
                handler.OnStartVertex(graph, source);
                TraverseCore(graph, source, colorByVertex, handler, cancellationToken);
            }
        }
    }
}
