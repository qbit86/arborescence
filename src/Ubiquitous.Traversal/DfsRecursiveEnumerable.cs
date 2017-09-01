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
            return TraverseCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<TVertex, TEdge>> TraverseCoroutine()
        {
            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
