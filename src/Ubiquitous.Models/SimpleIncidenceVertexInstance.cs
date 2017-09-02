namespace Ubiquitous
{
    public struct SimpleIncidenceVertexInstance<TEdges> : IIncidenceVertexDataConcept<TEdges, TEdges>
    {
        public TEdges GetOutEdges(TEdges vertexData) => vertexData;
    }
}
