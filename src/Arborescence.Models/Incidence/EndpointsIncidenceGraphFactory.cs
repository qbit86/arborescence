namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;

    public static class EndpointsIncidenceGraphFactory<TVertex>
        where TVertex : notnull
    {
        public static EndpointsIncidenceGraph<TVertex, Dictionary<TVertex, List<Endpoints<TVertex>>>>
            Create()
        {
            Dictionary<TVertex, List<Endpoints<TVertex>>> outEdgesByVertex = new();
            return new(outEdgesByVertex);
        }

        public static EndpointsIncidenceGraph<TVertex, Dictionary<TVertex, List<Endpoints<TVertex>>>>
            Create(IEqualityComparer<TVertex>? vertexComparer)
        {
            Dictionary<TVertex, List<Endpoints<TVertex>>> outEdgesByVertex = new(vertexComparer);
            return new(outEdgesByVertex);
        }

        public static EndpointsIncidenceGraph<TVertex, Dictionary<TVertex, List<Endpoints<TVertex>>>>
            CreateUnchecked(Dictionary<TVertex, List<Endpoints<TVertex>>> outEdgesByVertex)
        {
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));

            return new(outEdgesByVertex);
        }
    }
}
