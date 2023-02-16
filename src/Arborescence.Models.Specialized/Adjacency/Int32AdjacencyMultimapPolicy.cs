#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;

    internal readonly struct Int32AdjacencyMultimapPolicy :
        IMultimapPolicy<int, int[], ArraySegment<int>.Enumerator>
    {
        public ArraySegment<int>.Enumerator GetEnumerator(int[] multimap, int key)
        {
            if (multimap is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            throw new NotImplementedException();
        }
    }
}
#endif
