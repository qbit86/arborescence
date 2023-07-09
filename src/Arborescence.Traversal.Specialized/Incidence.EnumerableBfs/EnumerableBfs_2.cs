namespace Arborescence.Traversal.Specialized.Incidence
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableBfs<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<int>
    {
        public static IEnumerator<int> EnumerateVertices<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator> =>
            EnumerateVerticesChecked(graph, source, vertexCount);

        internal static IEnumerator<int> EnumerateVerticesChecked<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            throw new NotImplementedException();
        }
    }
}
