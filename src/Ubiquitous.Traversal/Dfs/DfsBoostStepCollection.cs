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
            private StackFrame _poppedStackFrame;
            private TEdges _edges;

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object System.Collections.IEnumerator.Current => _current;

            internal Enumerator(DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> steps)
            {
                _steps = steps;
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _stack = new Stack<StackFrame>();
                _poppedStackFrame = default(StackFrame);
                _edges = default(TEdges);
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
                                _steps.ColorMap[_steps.StartVertex] = Color.Gray;
                                _current = Step.Create(DfsStepKind.DiscoverVertex, _steps.StartVertex, default(TEdge));
                                _state = 1;
                                return true;
                            }
                        case 1:
                            {
                                TEdges edges;
                                bool hasOutEdges = _steps.VertexConcept.TryGetOutEdges(_steps.Graph, _steps.StartVertex, out edges);
                                if (!hasOutEdges)
                                {
                                    _steps.ColorMap[_steps.StartVertex] = Color.Gray;
                                    _current = Step.Create(DfsStepKind.FinishVertex, _steps.StartVertex, default(TEdge));
                                    _state = int.MaxValue;
                                    return true;
                                }
                                var pushingStackFrame = new StackFrame(_steps.StartVertex, false, default(TEdge), edges);
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
                                _poppedStackFrame = _stack.Pop();
                                if (_poppedStackFrame.HasEdge)
                                {
                                    _current = Step.Create(DfsStepKind.FinishEdge, default(TVertex), _poppedStackFrame.Edge);
                                    _state = 3;
                                    return true;
                                }
                                _state = 3;
                                continue;
                            }
                        case 3:
                            {
                                _edges = _poppedStackFrame.EdgeEnumerator;
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
                                // TODO:
                                throw new System.NotImplementedException();
                            }
                        case short.MaxValue:
                            {
                                _steps.ColorMap[_steps.StartVertex] = Color.Black;
                                _current = Step.Create(DfsStepKind.FinishVertex, _poppedStackFrame.Vertex, default(TEdge));
                                _state = 2;
                                return true;
                            }
                        case int.MaxValue:
                            {
                                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                                _state = -1;
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
