namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapFactory>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private TColorMapFactory _colorMapFactory;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TGraphConcept GraphConcept { get; }

        internal BaselineDfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TGraphConcept graphConcept, TColorMapFactory colorMapFactory)
        {
            Assert(colorMapFactory != null);

            Graph = graph;
            StartVertex = startVertex;
            GraphConcept = graphConcept;
            _colorMapFactory = colorMapFactory;
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
            TColorMap colorMap = _colorMapFactory.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

                var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                    TGraphConcept>(Graph, StartVertex, colorMap, GraphConcept);
                using (IEnumerator<Step<DfsStepKind, TVertex, TEdge>> stepEnumerator = steps.GetEnumerator())
                {
                    while (stepEnumerator.MoveNext())
                        yield return stepEnumerator.Current;
                }
            }
            finally
            {
                _colorMapFactory.Release(Graph, colorMap);
            }
        }
    }
}
