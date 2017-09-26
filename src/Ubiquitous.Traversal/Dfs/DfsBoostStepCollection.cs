namespace Ubiquitous.Dfs
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TColorMap ColorMap { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        public DfsBoostStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(colorMap != null);
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            ColorMap = colorMap;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this, StartVertex);
        }

        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> IEnumerable<Step<DfsStepKind, TVertex, TEdge>>.GetEnumerator()
        {
            Enumerator result = GetEnumerator();
            return result;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Enumerator result = GetEnumerator();
            return result;
        }


        // http://www.boost.org/doc/libs/1_65_1/boost/graph/depth_first_search.hpp
        internal struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private readonly DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> _steps;
            private readonly TVertex _vertex;
            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object System.Collections.IEnumerator.Current => _current;

            public Enumerator(DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> steps, TVertex vertex)
            {
                _steps = steps;
                _vertex = vertex;
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                throw new System.NotImplementedException();
            }

            public void Reset()
            {
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }
        }
    }
}
