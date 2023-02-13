namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;

    public static class EdgeIdentityGraphFactory<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : notnull
    {
        public static EdgeIdentityGraph<TVertex, TEdge, Dictionary<TEdge, TVertex>, Dictionary<TVertex, List<TEdge>>>
            Create()
        {
            Dictionary<TEdge, TVertex> tailByEdge = new();
            Dictionary<TEdge, TVertex> headByEdge = new();
            Dictionary<TVertex, List<TEdge>> outEdgesByVertex = new();
            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }
    }
}
