namespace Ubiquitous
{
    internal interface IStepEnumeratorProviderConcept<TGraph, TVertex, TColorMap, TSteps, TVertexConcept, TEdgeConcept>
    {
        TSteps GetEnumerator(TGraph graph, TVertex vertex, TColorMap colorMap,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept);
    }
}
