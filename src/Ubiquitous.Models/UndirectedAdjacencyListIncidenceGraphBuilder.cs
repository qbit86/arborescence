namespace Ubiquitous.Models
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct UndirectedAdjacencyListIncidenceGraphBuilder :
        IGraphBuilder<UndirectedAdjacencyListIncidenceGraph, int, int>
    {
        public bool TryAdd(int source, int target, out int edge) => throw new NotImplementedException();

        public UndirectedAdjacencyListIncidenceGraph ToGraph() => throw new NotImplementedException();
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
