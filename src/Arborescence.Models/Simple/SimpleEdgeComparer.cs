namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal sealed class SimpleEdgeComparer : IComparer<Int32Endpoints>
    {
        internal static SimpleEdgeComparer Instance { get; } = new();

        public int Compare(Int32Endpoints x, Int32Endpoints y)
        {
            int left = x.Tail;
            int right = y.Tail;
            return left.CompareTo(right);
        }
    }
}
