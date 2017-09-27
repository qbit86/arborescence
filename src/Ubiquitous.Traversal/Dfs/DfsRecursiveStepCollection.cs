namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    [System.Obsolete("Use `DfsBaselineStepCollection<...>` for recursive DFS.")]
    internal struct DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
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

        public DfsRecursiveStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
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

        public ProcessVertexEnumerator GetEnumerator()
        {
            return new ProcessVertexEnumerator(this, StartVertex);
        }

        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> IEnumerable<Step<DfsStepKind, TVertex, TEdge>>.GetEnumerator()
        {
            ProcessVertexEnumerator result = GetEnumerator();
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ProcessVertexEnumerator result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> ProcessEdgeCoroutine(TEdge edge)
        {
            yield return Step.Create(DfsStepKind.ExamineEdge, default(TVertex), edge);

            TVertex target;
            if (EdgeConcept.TryGetTarget(Graph, edge, out target))
            {
                Color neighborColor;
                if (!ColorMap.TryGetValue(target, out neighborColor))
                    neighborColor = Color.None;

                switch (neighborColor)
                {
                    case Color.None:
                    case Color.White:
                        yield return Step.Create(DfsStepKind.TreeEdge, default(TVertex), edge);
                        var steps = new ProcessVertexEnumerator(this, target);
                        while (steps.MoveNext())
                            yield return steps.Current;
                        break;
                    case Color.Gray:
                        yield return Step.Create(DfsStepKind.BackEdge, default(TVertex), edge);
                        break;
                    default:
                        yield return Step.Create(DfsStepKind.ForwardOrCrossEdge, default(TVertex), edge);
                        break;
                }
            }

            yield return Step.Create(DfsStepKind.FinishEdge, default(TVertex), edge);
        }

        // http://csharpindepth.com/Articles/Chapter6/IteratorBlockImplementation.aspx
        internal struct ProcessVertexEnumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private readonly DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> _steps;
            private readonly TVertex _vertex;
            private TEdges _edges;
            private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> _innerEnumerator;
            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;

            public ProcessVertexEnumerator(
                DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                    TVertexConcept, TEdgeConcept> steps,
                TVertex vertex)
            {
                _steps = steps;
                _vertex = vertex;
                _edges = default(TEdges);
                _innerEnumerator = default(IEnumerator<Step<DfsStepKind, TVertex, TEdge>>);
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                            return DiscoverVertex();
                        case 1:
                            if (!_steps.VertexConcept.TryGetOutEdges(_steps.Graph, _vertex, out _edges) || _edges == null)
                                return FinishVertex();
                            _state = 2;
                            continue;
                        case 2:
                            if (!_edges.MoveNext())
                                return FinishVertex();
                            _state = 3;
                            continue;
                        case 3:
                            TEdge edge = _edges.Current;
                            _innerEnumerator = _steps.ProcessEdgeCoroutine(edge);
                            _state = 4;
                            continue;
                        case 4:
                            if (!_innerEnumerator.MoveNext())
                            {
                                _state = 2;
                                continue;
                            }
                            _state = 5;
                            continue;
                        case 5:
                            _current = _innerEnumerator.Current;
                            _state = 4;
                            return true;
                        default:
                            return Terminate();
                    }
                }
            }

            private bool DiscoverVertex()
            {
                _steps.ColorMap[_vertex] = Color.Gray;
                _current = Step.Create(DfsStepKind.DiscoverVertex, _vertex, default(TEdge));
                _state = 1;
                return true;
            }

            private bool FinishVertex()
            {
                _steps.ColorMap[_vertex] = Color.Black;
                _current = Step.Create(DfsStepKind.FinishVertex, _vertex, default(TEdge));
                _state = int.MaxValue;
                return true;
            }

            private bool Terminate()
            {
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = -1;
                return false;
            }

            public void Reset()
            {
                throw new System.NotSupportedException();
            }
        }
    }
}
