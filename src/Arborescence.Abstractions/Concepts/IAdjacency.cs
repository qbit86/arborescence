namespace Arborescence
{
    // https://www.boost.org/doc/libs/1_75_0/libs/graph/doc/AdjacencyGraph.html

    public interface IAdjacency<in TVertex, out TVertices>
    {
        TVertices EnumerateAdjacentVertices(TVertex vertex);
    }
}
