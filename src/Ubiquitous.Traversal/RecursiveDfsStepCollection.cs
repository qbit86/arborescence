namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct RecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept> Impl { get; }

        private TVertex StartVertex { get; }

        internal TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal RecursiveDfsStepCollection(RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept> impl, TVertex startVertex, TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(startVertex != null);

            Impl = impl;
            StartVertex = startVertex;
            ColorMapFactoryConcept = colorMapFactoryConcept;
        }

        public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator()
        {
            return GetEnumeratorCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<DfsStepKind, TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumeratorCoroutine()
        {
            yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Impl.Graph);
            if (colorMap == null)
                yield break;

            try
            {
                IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = Impl.ProcessVertexCoroutine(StartVertex, colorMap);
                foreach (var step in steps)
                    yield return step;
            }
            finally
            {
                ColorMapFactoryConcept.Release(Impl.Graph, colorMap);
            }
        }
    }
}
