#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
#if NET5_0_OR_GREATER
    using System.Collections.Generic;
#endif
    using System;

    public static class Int32FrozenIncidenceGraphFactory
    {
        public static Int32FrozenIncidenceGraph FromEdges(int vertexCount, Endpoints<int>[] endpointsByEdge)
        {
            if (endpointsByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(endpointsByEdge));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            throw new NotImplementedException();
        }

#if NET5_0_OR_GREATER
        public static Int32FrozenIncidenceGraph FromEdges(int vertexCount, Span<Endpoints<int>> endpointsByEdge)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            throw new NotImplementedException();
        }

        public static Int32FrozenAdjacencyGraph FromEdges(int vertexCount, List<Endpoints<int>> endpointsByEdge)
        {
            if (endpointsByEdge is null)
                throw new ArgumentNullException(nameof(endpointsByEdge));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            throw new NotImplementedException();
        }
#endif
    }
}
#endif
