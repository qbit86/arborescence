#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics;

    internal readonly struct Int32AdjacencyMultimapPolicy :
        IMultimapPolicy<int, int[], ArraySegment<int>.Enumerator>
    {
        public ArraySegment<int>.Enumerator GetEnumerator(int[] multimap, int key)
        {
            if (multimap is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            Debug.Assert(multimap.Length >= 2, "multimap.Length >= 2");
            int vertexCount = multimap[0];
            Debug.Assert(vertexCount >= 0, "vertexCount >= 0");
            if (unchecked((uint)key >= (uint)vertexCount))
                return ArraySegment<int>.Empty.GetEnumerator();

            int edgeCount = multimap[1];
            Debug.Assert(edgeCount >= 0, "edgeCount >= 0");
            ReadOnlySpan<int> upperBoundByVertex = multimap.AsSpan(2, vertexCount);
            int upperBound = upperBoundByVertex[key];
            int lowerBound = key == 0 ? 0 : upperBoundByVertex[key - 1];
            int offset = 2 + vertexCount;
            ArraySegment<int> segment = new(multimap, offset, upperBound - lowerBound);
            return segment.GetEnumerator();
        }
    }
}
#endif
