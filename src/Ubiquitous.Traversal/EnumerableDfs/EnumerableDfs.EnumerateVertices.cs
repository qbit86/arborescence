namespace Ubiquitous.Traversal
{
    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        private readonly struct VertexInfo
        {
            private readonly TVertex _exploredVertex;
            private readonly bool _hasInEdge;

            internal VertexInfo(TVertex exploredVertex, bool hasInEdge)
            {
                _exploredVertex = exploredVertex;
                _hasInEdge = hasInEdge;
            }

            internal TVertex ExploredVertex => _exploredVertex;
            internal bool HasInEdge => _hasInEdge;
        }
    }
    // ReSharper restore UnusedTypeParameter
}
