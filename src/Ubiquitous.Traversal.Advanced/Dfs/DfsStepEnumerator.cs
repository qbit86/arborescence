// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    // http://www.boost.org/doc/libs/1_65_1/boost/graph/depth_first_search.hpp
    internal struct DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapPolicy<TColorMap, TVertex, Color>
    {
        private Step<DfsStepKind, TVertex, TEdge> _current;
        private int _state;

        private readonly TGraph _graph;
        private TEdgeEnumerator _edgeEnumerator; // Corresponds to iterator range in Boost implementation.
        private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
        private TVertex _currentVertex; // Corresponds to `u` in Boost implementation.

        private TColorMap _colorMap;
        private readonly List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
        private TGraphConcept _graphConcept;
        private TColorMapConcept _colorMapConcept;

        internal DfsStepEnumerator(TGraph graph, TVertex startVertex,
            TColorMap colorMap, List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> stack,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            Assert(colorMap != null);
            Assert(graphConcept != null);

            _graph = graph;
            _colorMap = colorMap;
            _stack = stack;
            _graphConcept = graphConcept;
            _colorMapConcept = colorMapConcept;

            _current = default(Step<DfsStepKind, TVertex, TEdge>);
            _state = 0;

            _edgeEnumerator = default(TEdgeEnumerator);
            _neighborVertex = default(TVertex);
            _currentVertex = startVertex;
        }

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public Step<DfsStepKind, TVertex, TEdge> Current => _current;

        public bool MoveNext()
        {
            // Conventions for state “naming” is taken from here:
            // http://csharpindepth.com/Articles/Chapter6/IteratorBlockImplementation.aspx
            // • -1: "After" - the iterator has finished, either by reaching the end of the method or by hitting yield break
            // • 0: "Before" - MoveNext() hasn't been called yet
            // • Anything positive: indicates where to resume from.

            // With `while (true)` we can avoid `goto label`,
            // simulating the latter with `_state = label; continue;`.
            while (true)
            {
                switch (_state)
                {
                    case 0:
                    {
                        _colorMapConcept.TryPut(_colorMap, _currentVertex, Color.Gray);
                        _current = CreateVertexStep(DfsStepKind.DiscoverVertex, _currentVertex);
                        _state = 1;
                        return true;
                    }
                    case 1:
                    {
                        bool hasOutEdges =
                            _graphConcept.TryGetOutEdges(_graph, _currentVertex, out TEdgeEnumerator edges);
                        if (!hasOutEdges)
                        {
                            _colorMapConcept.TryPut(_colorMap, _currentVertex, Color.Black);
                            _current = CreateVertexStep(DfsStepKind.FinishVertex, _currentVertex);
                            _state = int.MaxValue;
                            return true;
                        }

                        DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> pushingStackFrame =
                            CreateVertexStackFrame(_currentVertex, edges);
                        _stack.Add(pushingStackFrame);
                        _state = 2;
                        continue;
                    }
                    case 2:
                    {
                        if (_stack.Count <= 0)
                        {
                            _state = int.MaxValue;
                            continue;
                        }

                        DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> poppedStackFrame = _stack[_stack.Count - 1];
                        _stack.RemoveAt(_stack.Count - 1);
                        _currentVertex = poppedStackFrame.Vertex;
                        _edgeEnumerator = poppedStackFrame.EdgeEnumerator;
                        if (poppedStackFrame.HasEdge)
                        {
                            _current = CreateEdgeStep(DfsStepKind.FinishEdge, poppedStackFrame.Edge);
                            _state = 3;
                            return true;
                        }

                        _state = 3;
                        continue;
                    }
                    case 3:
                    {
                        if (!_edgeEnumerator.MoveNext())
                        {
                            _state = short.MaxValue;
                            continue;
                        }

                        bool isValid = _graphConcept.TryGetTarget(_graph, _edgeEnumerator.Current, out _neighborVertex);
                        if (!isValid)
                        {
                            _state = 3;
                            continue;
                        }

                        _current = CreateEdgeStep(DfsStepKind.ExamineEdge, _edgeEnumerator.Current);
                        _state = 4;
                        return true;
                    }
                    case 4:
                    {
                        if (!_colorMapConcept.TryGet(_colorMap, _neighborVertex, out Color neighborColor))
                            neighborColor = Color.None;
                        TEdge edge = _edgeEnumerator.Current;
                        switch (neighborColor)
                        {
                            case Color.None:
                            case Color.White:
                                _current = CreateEdgeStep(DfsStepKind.TreeEdge, edge);
                                _state = 5;
                                return true;
                            case Color.Gray:
                                _current = CreateEdgeStep(DfsStepKind.BackEdge, edge);
                                _state = 7;
                                return true;
                            default:
                                _current = CreateEdgeStep(DfsStepKind.ForwardOrCrossEdge, edge);
                                _state = 7;
                                return true;
                        }
                    }
                    case 5:
                    {
                        DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> pushingStackFrame =
                            CreateEdgeStackFrame(_currentVertex, _edgeEnumerator.Current, _edgeEnumerator);
                        _stack.Add(pushingStackFrame);
                        _currentVertex = _neighborVertex;
                        _colorMapConcept.TryPut(_colorMap, _currentVertex, Color.Gray);
                        _current = CreateVertexStep(DfsStepKind.DiscoverVertex, _currentVertex);
                        _state = 6;
                        return true;
                    }
                    case 6:
                    {
                        bool hasOutEdges = _graphConcept.TryGetOutEdges(_graph, _currentVertex, out _edgeEnumerator);
                        if (!hasOutEdges)
                        {
                            _state = short.MaxValue;
                            continue;
                        }

                        _state = 3;
                        continue;
                    }
                    case 7:
                    {
                        _current = CreateEdgeStep(DfsStepKind.FinishEdge, _edgeEnumerator.Current);
                        _state = 3;
                        return true;
                    }
                    case short.MaxValue:
                    {
                        _colorMapConcept.TryPut(_colorMap, _currentVertex, Color.Black);
                        _current = CreateVertexStep(DfsStepKind.FinishVertex, _currentVertex);
                        _state = 2;
                        return true;
                    }
                    case int.MaxValue:
                    {
                        _current = default(Step<DfsStepKind, TVertex, TEdge>);
                        _state = -1;
                        return false;
                    }
                }

                return false;
            }
        }

        private static Step<DfsStepKind, TVertex, TEdge> CreateVertexStep(DfsStepKind kind, TVertex vertex)
        {
            return new Step<DfsStepKind, TVertex, TEdge>(kind, vertex, default(TEdge));
        }

        private static Step<DfsStepKind, TVertex, TEdge> CreateEdgeStep(DfsStepKind kind, TEdge edge)
        {
            return new Step<DfsStepKind, TVertex, TEdge>(kind, default(TVertex), edge);
        }

        private static DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> CreateVertexStackFrame(
            TVertex vertex, TEdgeEnumerator edgeEnumerator)
        {
            return new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, false, default(TEdge), edgeEnumerator);
        }

        private static DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> CreateEdgeStackFrame(
            TVertex vertex, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            return new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, true, edge, edgeEnumerator);
        }
    }
}
