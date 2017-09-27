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
        internal struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private readonly DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> _steps;
            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;
            private Stack<StackFrame> _stack;

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object System.Collections.IEnumerator.Current => _current;

            public Enumerator(DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept> steps)
            {
                _steps = steps;
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _stack = new Stack<StackFrame>();
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
                            _steps.ColorMap[_steps.StartVertex] = Color.Gray;
                            _current = Step.Create(DfsStepKind.DiscoverVertex, _steps.StartVertex, default(TEdge));
                            _state = 1;
                            return true;
                        case 1:
                            TEdges edges;
                            bool hasOutEdges = _steps.VertexConcept.TryGetOutEdges(_steps.Graph, _steps.StartVertex, out edges);
                            if (!hasOutEdges)
                            {
                                _steps.ColorMap[_steps.StartVertex] = Color.Gray;
                                _current = Step.Create(DfsStepKind.FinishVertex, _steps.StartVertex, default(TEdge));
                                _state = int.MaxValue;
                                return true;
                            }
                            var stackFrame = new StackFrame(StackFrameKind.None, _steps.StartVertex, edges);
                            _stack.Push(stackFrame);
                            _state = 2;
                            continue;
                        case 2:
                            // TODO:
                        default:
                            _current = default(Step<DfsStepKind, TVertex, TEdge>);
                            _state = -1;
                            return false;
                    }
                }
            }

            public void Reset()
            {
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
            }
        }

        internal enum StackFrameKind
        {
            None = 0,
            Some = 1,
        }

        internal struct StackFrame
        {
            internal StackFrameKind Kind { get; }

            internal TVertex Vertex { get; }

            internal TEdges EdgeEnumerator { get; }

            // internal TEdge Edge { get; }

            internal StackFrame(StackFrameKind kind, TVertex vertex, TEdges edgeEnumerator /*, TEdge edge */)
            {
                Kind = kind;
                Vertex = vertex;
                EdgeEnumerator = edgeEnumerator;
                // Edge = edge;
            }
        }
    }
}
