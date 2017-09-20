namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal sealed class DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap, TTraversal,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TTraversal : ITraversal<TVertex, TEdge, TColorMap>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TTraversal Traversal { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsTreeStepCollection(TGraph graph, TVertex startVertex, TTraversal traversal,
            TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(traversal != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            Traversal = traversal;
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

                IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = Traversal.Traverse(StartVertex, colorMap);
                foreach (var step in steps)
                    yield return step;
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
