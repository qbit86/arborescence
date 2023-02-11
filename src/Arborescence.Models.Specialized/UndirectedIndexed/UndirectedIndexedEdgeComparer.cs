namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal sealed class UndirectedIndexedEdgeComparer : IComparer<int>
    {
        private readonly int[] _headByEdge;
        private readonly int[] _tailByEdge;

        internal UndirectedIndexedEdgeComparer(int[] tailByEdge, int[] headByEdge)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
        }

        public int Compare(int x, int y)
        {
            int leftIndex = x < 0 ? ~x : x;
            int[] leftTailByEdge = x < 0 ? _headByEdge : _tailByEdge;
            int leftTail = leftTailByEdge[leftIndex];
            int rightIndex = y < 0 ? ~y : y;
            int[] rightTailByEdge = y < 0 ? _headByEdge : _tailByEdge;
            int rightTail = rightTailByEdge[rightIndex];
            return leftTail.CompareTo(rightTail);
        }
    }
}
