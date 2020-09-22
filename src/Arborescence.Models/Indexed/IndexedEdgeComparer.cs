namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class IndexedEdgeComparer : IComparer<int>
    {
        private readonly int[] _tailByEdge;

        public IndexedEdgeComparer(int[] tailByEdge)
        {
            Debug.Assert(tailByEdge != null, nameof(tailByEdge) + " != null");
            _tailByEdge = tailByEdge;
        }

        public int Compare(int x, int y)
        {
            int leftTail = _tailByEdge[x];
            int rightTail = _tailByEdge[y];
            return leftTail.CompareTo(rightTail);
        }
    }
}
