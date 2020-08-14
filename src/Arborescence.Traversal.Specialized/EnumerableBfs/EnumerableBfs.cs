namespace Arborescence.Traversal.Specialized
{
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct EnumerableBfs<TGraph, TEdge, TEdgeEnumerator>
        where TGraph : IOutEdgesConcept<int, TEdgeEnumerator>, IHeadConcept<int, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
