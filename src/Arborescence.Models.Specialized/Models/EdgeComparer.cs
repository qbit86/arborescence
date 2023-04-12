#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal sealed class EdgeComparer : IComparer<Endpoints<int>>
    {
        internal static EdgeComparer Instance { get; } = new();

        public int Compare(Endpoints<int> x, Endpoints<int> y) => x.Tail.CompareTo(y.Tail);
    }
}
#endif
