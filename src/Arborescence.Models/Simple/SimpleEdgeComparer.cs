namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal sealed class SimpleEdgeComparer : IComparer<Endpoints>
    {
        internal static SimpleEdgeComparer Instance { get; } = new();

        public int Compare(Endpoints x, Endpoints y)
        {
            int left = x.Tail;
            int right = y.Tail;
            return left.CompareTo(right);
        }
    }
}
