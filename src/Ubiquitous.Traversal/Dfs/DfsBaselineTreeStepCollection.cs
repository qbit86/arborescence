namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsBaselineTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsBaselineTreeStepCollection(TGraph graph, TVertex startVertex,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
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
            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

                var steps = new DfsBaselineStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                    TVertexConcept, TEdgeConcept>(Graph, StartVertex, colorMap, VertexConcept, EdgeConcept);
                IEnumerator<Step<DfsStepKind, TVertex, TEdge>> stepEnumerator = steps.GetEnumerator();
                while (stepEnumerator.MoveNext())
                    yield return stepEnumerator.Current;
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
