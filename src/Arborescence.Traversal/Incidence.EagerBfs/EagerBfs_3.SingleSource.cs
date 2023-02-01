namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EagerBfs<TVertex, TEdge, TEdgeEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge> =>
            TraverseChecked(graph, source, colorByVertex, handler);

        internal static void TraverseChecked<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (source is null)
                ThrowHelper.ThrowArgumentNullException(nameof(source));

            if (colorByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            colorByVertex[source] = Color.Gray;
            handler.OnDiscoverVertex(graph, source);
            var queue = new ValueQueue<TVertex>();
            try
            {
                queue.Add(source);
                Traverse(graph, ref queue, colorByVertex, handler);
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }
    }
}
