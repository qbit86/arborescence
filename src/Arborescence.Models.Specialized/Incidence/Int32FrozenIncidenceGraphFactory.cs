#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
#if NET5_0_OR_GREATER
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
#endif

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

            // We cannot reuse this overload taking span:
            // Int32FrozenIncidenceGraphFactory.FromEdgesUnchecked(int, Span<Endpoints<int>>)
            // Because the sorting algorithm that takes spans is only available in .NET 5 and later:
            // MemoryExtensions.Sort<TKey,TValue,TComparer>(Span<TKey>, Span<TValue>, TComparer)
            // https://learn.microsoft.com/en-us/dotnet/api/system.memoryextensions.sort?view=net-6.0

            // We need to implement sorting independently with another overload that takes arrays:
            // Int32FrozenIncidenceGraphFactory.FromEdgesUnchecked(int, Endpoints<int>[])
            // Because the sorting algorithm that takes arrays is available on older versions of .NET:
            // Array.Sort<TKey,TValue>(TKey[], TValue[], IComparer<TKey>)
            // https://learn.microsoft.com/en-us/dotnet/api/system.array.sort?view=net-6.0

            return FromEdgesUnchecked(vertexCount, endpointsByEdge);
        }

        private static Int32FrozenIncidenceGraph FromEdgesUnchecked(
            int vertexCount, Endpoints<int>[] endpointsByEdge) =>
            throw new NotImplementedException();

#if NET5_0_OR_GREATER
        public static Int32FrozenIncidenceGraph FromEdges(int vertexCount, Span<Endpoints<int>> endpointsByEdge)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            return FromEdgesUnchecked(vertexCount, endpointsByEdge);
        }

        public static Int32FrozenIncidenceGraph FromEdges(int vertexCount, List<Endpoints<int>> endpointsByEdge)
        {
            if (endpointsByEdge is null)
                throw new ArgumentNullException(nameof(endpointsByEdge));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            return FromEdgesUnchecked(vertexCount, CollectionsMarshal.AsSpan(endpointsByEdge));
        }

        private static Int32FrozenIncidenceGraph FromEdgesUnchecked(
            int vertexCount, Span<Endpoints<int>> endpointsByEdge) =>
            throw new NotImplementedException();
#endif
    }
}
#endif
