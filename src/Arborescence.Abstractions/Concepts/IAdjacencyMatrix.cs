namespace Arborescence
{
    // https://www.boost.org/doc/libs/1_75_0/libs/graph/doc/AdjacencyMatrix.html

    public interface IAdjacencyMatrix<in TVertex, TEdge>
    {
        bool TryGetEdge(TVertex tail, TVertex head, out TEdge edge);
    }
}
