namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TColorMapConcept _colorMapConcept;

        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TGraphConcept GraphConcept { get; }

        internal BaselineDfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            Assert(colorMapConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            GraphConcept = graphConcept;
            _colorMapConcept = colorMapConcept;
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
            TColorMap colorMap = _colorMapConcept.Acquire();
            if (colorMap == null)
                yield break;

            try
            {
                yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

                var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                    TGraphConcept, TColorMapConcept>(Graph, StartVertex, colorMap, GraphConcept, _colorMapConcept);
                using (IEnumerator<Step<DfsStepKind, TVertex, TEdge>> stepEnumerator = steps.GetEnumerator())
                {
                    while (stepEnumerator.MoveNext())
                        yield return stepEnumerator.Current;
                }
            }
            finally
            {
                _colorMapConcept.Release(colorMap);
            }
        }
    }
}
