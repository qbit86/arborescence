// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    // States:
    // • -2: "After" — the iterator has finished
    // • -1: disposed
    // • 0: not initialized
    // • 1: "Before" — MoveNext() hasn't been called yet
    // • Anything positive: indicates where to resume from
    // --- https://csharpindepth.com/Articles/IteratorBlockImplementation

    internal struct DfsVertexIterator<
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

        internal DfsStep<TVertex> _current;
        private TEdgeEnumerator _edgeEnumerator; // Corresponds to iterator range in Boost implementation.
        private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
        private TVertex _currentVertex; // Corresponds to `u` in Boost implementation.
        private List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
        private int _state;

        internal DfsVertexIterator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
            _graph = graph;
            _colorMap = colorMap;

            _current = new DfsStep<TVertex>(DfsStepKind.None, startVertex);
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
                        return CreateDiscoverVertexStep(_currentVertex, 2, out _current);
                    case 2:
                        EnsureStackInitialized();
                        TEdgeEnumerator edges = _graphPolicy.EnumerateOutEdges(_graph, _currentVertex);
                        PushVertexStackFrame(_currentVertex, edges);
                        _state = 3;
                        continue;
                    case 3:
                        if (!TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame))
                            return Terminate(out _current);

                        _currentVertex = stackFrame.Vertex;
                        _edgeEnumerator = stackFrame.EdgeEnumerator;
                        _state = 4;
                        continue;
                    case 4:
                        if (!_edgeEnumerator.MoveNext())
                            return CreateFinishVertexStep(_currentVertex, 3, out _current);

                        bool isValid = _graphPolicy.TryGetTarget(_graph, _edgeEnumerator.Current, out _neighborVertex);
                        if (!isValid)
                            continue;

                        _state = 5;
                        continue;
                    case 5:
                        Color neighborColor = GetColorOrDefault(_colorMap, _neighborVertex);
                        switch (neighborColor)
                        {
                            case Color.None:
                            case Color.White:
                                _state = 6;
                                continue;
                            case Color.Gray:
                                _state = 4;
                                continue;
                            default:
                                _state = 4;
                                continue;
                        }
                    case 6:
                        PushEdgeStackFrame(_currentVertex, _edgeEnumerator.Current, _edgeEnumerator);
                        _currentVertex = _neighborVertex;
                        return CreateDiscoverVertexStep(_currentVertex, 7, out _current);
                    case 7:
                        _edgeEnumerator = _graphPolicy.EnumerateOutEdges(_graph, _currentVertex);
                        _state = 4;
                        continue;
                }

                return Terminate(out _current);
            }
        }

        internal void Reset(TVertex startVertex)
        {
            Assert(_state > 0, "_state > 0");

            _current = new DfsStep<TVertex>(DfsStepKind.None, startVertex);
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
        private bool CreateDiscoverVertexStep(TVertex vertex, int newState, out DfsStep<TVertex> current)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Gray);
            current = new DfsStep<TVertex>(DfsStepKind.DiscoverVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateFinishVertexStep(TVertex vertex, int newState, out DfsStep<TVertex> current)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Black);
            current = new DfsStep<TVertex>(DfsStepKind.FinishVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Terminate(out DfsStep<TVertex> current)
        {
            current = new DfsStep<TVertex>(DfsStepKind.None, _currentVertex);
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

// ReSharper restore FieldCanBeMadeReadOnly.Local
