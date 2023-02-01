namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EagerBfs<TVertex, TEdge, TEdgeEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge> =>
            TraverseChecked(graph, sources, colorByVertex, handler);

        internal static void TraverseChecked<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (colorByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            var queue = new ValueQueue<TVertex>();
            try
            {
                using IEnumerator<TVertex> sourceEnumerator = sources.GetEnumerator();
                while (sourceEnumerator.MoveNext())
                {
                    if (sourceEnumerator.Current is not { } source)
                        continue;
                    colorByVertex[source] = Color.Gray;
                    handler.OnDiscoverVertex(graph, source);
                    queue.Add(source);
                }

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
