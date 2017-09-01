namespace Ubiquitous
{
    public struct SimpleIncidenceVertexInstance<TEdge, TEdges> : IIncidenceVertexConcept<TEdge, TEdges, TEdges>
    {
        public TEdges GetOutEdges(TEdges vertexData) => vertexData;
    }
}
