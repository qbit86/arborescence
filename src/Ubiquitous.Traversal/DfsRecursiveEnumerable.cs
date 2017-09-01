namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public struct DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TColorMap, TColorMapFactoryConcept>
        : IEnumerable<Step<TVertex, TEdge>>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        public DfsRecursiveEnumerable(TGraph graph, TVertex startVertex)
        {
            Graph = graph;
            StartVertex = startVertex;
        }

        public IEnumerator<Step<TVertex, TEdge>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<TVertex, TEdge>> result = GetEnumerator();
            return result;
        }
    }
}
