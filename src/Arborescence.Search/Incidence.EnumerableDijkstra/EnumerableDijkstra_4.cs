namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;

    public static partial class EnumerableDijkstra<TVertex, TEdge, TEdgeEnumerator, TWeight>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
