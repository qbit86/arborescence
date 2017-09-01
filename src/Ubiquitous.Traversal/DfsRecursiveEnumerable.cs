namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public struct DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TColorMap, TColorMapFactoryConcept>
        : IEnumerable<Step<TVertex, TEdge>>

        where TColorMap : IDictionary<TVertex, Color>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        public DfsRecursiveEnumerable(TGraph graph, TVertex startVertex, TColorMapFactoryConcept colorMapFactoryConcept)
        {
            // Assert: `graph` != null.
            // Assert: `startVertex` != null.
            // Assert: `colorMapFactoryConcept` != null.

            Graph = graph;
            StartVertex = startVertex;
            ColorMapFactoryConcept = colorMapFactoryConcept;
        }

        public IEnumerator<Step<TVertex, TEdge>> GetEnumerator()
        {
            yield return Step.Create(StepKind.StartVertex, StartVertex, default(TEdge));

            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                var enumerator = TraverseCoroutine(colorMap);
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<TVertex, TEdge>> TraverseCoroutine(TColorMap colorMap)
        {
            throw new NotImplementedException();
        }
    }
}
