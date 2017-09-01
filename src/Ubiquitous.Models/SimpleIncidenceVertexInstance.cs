namespace Ubiquitous
{
    public struct SimpleIncidenceVertexInstance<TEdges> : IIncidenceVertexConcept<TEdges, TEdges>
    {
        public TEdges GetOutEdges(TEdges vertexData) => vertexData;
    }
}
