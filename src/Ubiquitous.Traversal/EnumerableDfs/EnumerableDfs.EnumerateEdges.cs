namespace Ubiquitous.Traversal
{
    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        private readonly struct EdgeInfo
        {
            private readonly TVertex _exploredVertex;
            private readonly TEdge _inEdge;
            private readonly bool _hasInEdge;

            internal EdgeInfo(TVertex exploredVertex)
            {
                _exploredVertex = exploredVertex;
                _inEdge = default;
                _hasInEdge = false;
            }

            internal EdgeInfo(TVertex exploredVertex, TEdge inEdge)
            {
                _exploredVertex = exploredVertex;
                _inEdge = inEdge;
                _hasInEdge = true;
            }

            internal TVertex ExploredVertex => _exploredVertex;

            internal bool TryGetInEdge(out TEdge inEdge)
            {
                inEdge = _inEdge;
                return _hasInEdge;
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
