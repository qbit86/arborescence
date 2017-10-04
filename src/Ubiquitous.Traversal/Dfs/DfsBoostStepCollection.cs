namespace Ubiquitous
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
            return new Enumerator(this);
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
        public struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private readonly DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> _steps;

            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;

            private readonly Stack<StackFrame> _stack;
            private TEdges _edges;  // Corresponds to iterator range in Boost implementation.
            private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
            private TVertex _currentVertex; // Corresponds to `u` in Boost implementation.

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object System.Collections.IEnumerator.Current => _current;

            internal Enumerator(DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> steps)
            {
                _steps = steps;
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _stack = new Stack<StackFrame>();
                _edges = default(TEdges);
                _neighborVertex = default(TVertex);
                _currentVertex = _steps.StartVertex;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                // With `while (true)` we can avoid `goto label`,
                // simulating the latter with `_state = label; continue;`.
                while (true)
                {
                    switch (_state)
                    {
                    case 0:
                        {
                            _steps.ColorMap[_currentVertex] = Color.Gray;
                            _current = Step.Create(DfsStepKind.DiscoverVertex, _currentVertex, default(TEdge));
                            _state = 1;
                            return true;
                        }
                    case 1:
                        {
                            TEdges edges;
                            bool hasOutEdges = _steps.VertexConcept.TryGetOutEdges(_steps.Graph, _currentVertex, out edges);
                            if (!hasOutEdges)
                            {
                                _steps.ColorMap[_currentVertex] = Color.Black;
                                _current = Step.Create(DfsStepKind.FinishVertex, _currentVertex, default(TEdge));
                                _state = int.MaxValue;
                                return true;
                            }
                            var pushingStackFrame = new StackFrame(_currentVertex, false, default(TEdge), edges);
                            _stack.Push(pushingStackFrame);
                            _state = 2;
                            continue;
                        }
                    case 2:
                        {
                            if (_stack.Count == 0)
                            {
                                _state = int.MaxValue;
                                continue;
                            }
                            StackFrame poppedStackFrame = _stack.Pop();
                            _currentVertex = poppedStackFrame.Vertex;
                            _edges = poppedStackFrame.EdgeEnumerator;
                            if (poppedStackFrame.HasEdge)
                            {
                                _current = Step.Create(DfsStepKind.FinishEdge, default(TVertex), poppedStackFrame.Edge);
                                _state = 4;
                                return true;
                            }
                            _state = 4;
                            continue;
                        }
                    case 4:
                        {
                            if (!_edges.MoveNext())
                            {
                                _state = short.MaxValue;
                                continue;
                            }
                            bool isValid = _steps.EdgeConcept.TryGetTarget(_steps.Graph, _edges.Current, out _neighborVertex);
                            if (!isValid)
                            {
                                _state = 4;
                                continue;
                            }
                            _current = Step.Create(DfsStepKind.ExamineEdge, default(TVertex), _edges.Current);
                            _state = 5;
                            return true;
                        }
                    case 5:
                        {
                            Color neighborColor;
                            if (!_steps.ColorMap.TryGetValue(_neighborVertex, out neighborColor))
                                neighborColor = Color.None;
                            TEdge edge = _edges.Current;
                            switch (neighborColor)
                            {
                            case Color.None:
                            case Color.White:
                                _current = Step.Create(DfsStepKind.TreeEdge, default(TVertex), edge);
                                _state = 6;
                                return true;
                            case Color.Gray:
                                _current = Step.Create(DfsStepKind.BackEdge, default(TVertex), edge);
                                _state = 8;
                                return true;
                            default:
                                _current = Step.Create(DfsStepKind.ForwardOrCrossEdge, default(TVertex), edge);
                                _state = 8;
                                return true;
                            }
                        }
                    case 6:
                        {
                            var pushingStackFrame = new StackFrame(_currentVertex, true, _edges.Current, _edges);
                            _stack.Push(pushingStackFrame);
                            _currentVertex = _neighborVertex;
                            _steps.ColorMap[_currentVertex] = Color.Gray;
                            _current = Step.Create(DfsStepKind.DiscoverVertex, _currentVertex, default(TEdge));
                            _state = 7;
                            return true;
                        }
                    case 7:
                        {
                            bool hasOutEdges = _steps.VertexConcept.TryGetOutEdges(_steps.Graph, _currentVertex, out _edges);
                            if (!hasOutEdges)
                            {
                                _state = short.MaxValue;
                                continue;
                            }
                            _state = 4;
                            continue;
                        }
                    case 8:
                        {
                            _current = Step.Create(DfsStepKind.FinishEdge, default(TVertex), _edges.Current);
                            _state = 4;
                            return true;
                        }
                    case short.MaxValue:
                        {
                            _steps.ColorMap[_currentVertex] = Color.Black;
                            _current = Step.Create(DfsStepKind.FinishVertex, _currentVertex, default(TEdge));
                            _state = 2;
                            return true;
                        }
                    case int.MaxValue:
                        {
                            _current = default(Step<DfsStepKind, TVertex, TEdge>);
                            _state = -1;
                            return false;
                        }
                    case -1:
                        {
                            return false;
                        }
                    default:
                        {
                            string message = $"{nameof(Enumerator)} is in unexpected state {_state}";
                            throw new System.InvalidOperationException(message);
                        }
                    }
                }
            }

            public void Reset()
            {
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }
        }

        internal struct StackFrame
        {
            internal TVertex Vertex { get; }

            internal bool HasEdge { get; }

            internal TEdge Edge { get; }

            internal TEdges EdgeEnumerator { get; }

            internal StackFrame(TVertex vertex, bool hasEdge, TEdge edge, TEdges edgeEnumerator)
            {
                Vertex = vertex;
                HasEdge = hasEdge;
                Edge = edge;
                EdgeEnumerator = edgeEnumerator;
            }
        }
    }
}
