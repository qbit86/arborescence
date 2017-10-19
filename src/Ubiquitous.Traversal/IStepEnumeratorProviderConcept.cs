namespace Ubiquitous
{
    internal interface IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap, out TSteps, TVertexConcept, TEdgeConcept>
    {
        TSteps GetEnumerator(TGraph graph, TVertex vertex, TColorMap colorMap,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept);
    }
}
