namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;

    public static partial class EagerDfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
