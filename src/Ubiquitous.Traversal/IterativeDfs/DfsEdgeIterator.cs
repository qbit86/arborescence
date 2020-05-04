// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    internal struct DfsEdgeIterator<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;
        private TGraph _graph;
        private TColorMap _colorMap;

        internal DfsStep<TEdge> _current;
        private TEdgeEnumerator _edgeEnumerator; // Corresponds to iterator range in Boost implementation.
        private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
        private TVertex _currentVertex;
        private List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
        private int _state;

        internal DfsEdgeIterator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
            _graph = graph;
            _colorMap = colorMap;

            _current = new DfsStep<TEdge>(DfsStepKind.None, default);
            _edgeEnumerator = default;
            _neighborVertex = default;
            _currentVertex = startVertex;
            _stack = default;
            _state = 1;
        }

        internal bool MoveNext()
        {
            if (_state <= 0)
                return false;

            // With `while (true)` we can avoid `goto label`,
            // simulating the latter with `_state = label; continue;`.
            while (true)
            {
                switch (_state)
                {
                    case 1:
                        _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Gray);
                        _state = 2;
                        continue;
                    case 2:
                        EnsureStackInitialized();
                        TEdgeEnumerator edges = _graphPolicy.EnumerateOutEdges(_graph, _currentVertex);
                        PushVertexStackFrame(_currentVertex, edges);
                        _state = 3;
                        continue;
                    case 3:
                        if (!TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame))
                            return Terminate();

                        _currentVertex = stackFrame.Vertex;
                        _edgeEnumerator = stackFrame.EdgeEnumerator;
                        // finish_edge has to be called here, not after the
                        // loop. Think of the pop as the return from a recursive call.
                        // --- boost/graph/depth_first_search.hpp
                        if (stackFrame.HasEdge)
                            return CreateEdgeStep(DfsStepKind.FinishEdge, stackFrame.Edge, 4);

                        _state = 4;
                        continue;
                    case 4:
                        if (!_edgeEnumerator.MoveNext())
                        {
                            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Black);
                            _state = 3;
                            continue;
                        }

                        if (!_graphPolicy.TryGetTarget(_graph, _edgeEnumerator.Current, out _neighborVertex))
                        {
                            _state = 4;
                            continue;
                        }

                        return CreateEdgeStep(DfsStepKind.ExamineEdge, _edgeEnumerator.Current, 5);
                    case 5:
                        throw new NotImplementedException();
                }

                return Terminate();
            }
        }

        internal void Reset(TVertex startVertex)
        {
            Assert(_state > 0, "_state > 0");

            _current = new DfsStep<TEdge>(DfsStepKind.None, default);
            _edgeEnumerator = default;
            _neighborVertex = default;
            _currentVertex = startVertex;
            _stack?.Clear();
            _state = 1;
        }

        internal void Dispose()
        {
            if (_state == -1)
                return;

            _current = default;
            _edgeEnumerator = default;
            _neighborVertex = default;
            _currentVertex = default;
            _stack?.Clear();
            _stack = default;
            _state = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateEdgeStep(DfsStepKind kind, TEdge edge, int newState)
        {
            _current = new DfsStep<TEdge>(kind, edge);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Terminate()
        {
            _current = new DfsStep<TEdge>(DfsStepKind.None, default);
            _state = -2;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureStackInitialized()
        {
            if (_stack is null)
                _stack = new List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>();
            else
                _stack.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushVertexStackFrame(TVertex vertex, TEdgeEnumerator edgeEnumerator)
        {
            var stackFrame = new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, false, default, edgeEnumerator);
            _stack.Add(stackFrame);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushEdgeStackFrame(TVertex vertex, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            var stackFrame = new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, true, edge, edgeEnumerator);
            _stack.Add(stackFrame);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame)
        {
            if (_stack.Count == 0)
            {
                stackFrame = default;
                return false;
            }

            stackFrame = _stack[_stack.Count - 1];
            _stack.RemoveAt(_stack.Count - 1);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            _colorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}
