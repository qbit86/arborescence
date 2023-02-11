namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal sealed class IndexedEdgeComparer : IComparer<int>
    {
        private readonly int[] _tailByEdge;

        internal IndexedEdgeComparer(int[] tailByEdge) => _tailByEdge = tailByEdge;

        public int Compare(int x, int y)
        {
            int leftTail = _tailByEdge[x];
            int rightTail = _tailByEdge[y];
            return leftTail.CompareTo(rightTail);
        }
    }
}
