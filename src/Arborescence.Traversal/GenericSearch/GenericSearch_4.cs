namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
