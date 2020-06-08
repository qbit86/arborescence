// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
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
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> : IDisposable
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IHeadPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;
        private TGraph _graph;
        private TColorMap _colorMap;

        private DfsStep<TVertex> _current;
        private TEdgeEnumerator _edgeEnumerator; // Corresponds to iterator range in Boost implementation.
        private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
        private TVertex _currentVertex; // Corresponds to `u` in Boost implementation.
        private Internal.Stack<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
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

        internal DfsStep<TVertex> Current => _current;

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
                        return UpdateColorMapAndCreateDiscoverVertexStep(_currentVertex, 2);
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
                        _state = 4;
                        continue;
                    case 4:
                        if (!_edgeEnumerator.MoveNext())
                            return UpdateColorMapAndCreateFinishVertexStep(_currentVertex, 3);

                        if (!_graphPolicy.TryGetHead(_graph, _edgeEnumerator.Current, out _neighborVertex))
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
                        return UpdateColorMapAndCreateDiscoverVertexStep(_currentVertex, 7);
                    case 7:
                        _edgeEnumerator = _graphPolicy.EnumerateOutEdges(_graph, _currentVertex);
                        _state = 4;
                        continue;
                }

                return Terminate();
            }
        }

        internal void Reset(TVertex startVertex)
        {
            Assert(_state > 0, "_state > 0");

            _current = new DfsStep<TVertex>(DfsStepKind.None, startVertex);
            _edgeEnumerator = default;
            _neighborVertex = default;
            _currentVertex = startVertex;
            EnsureStackReleased();
            _state = 1;
        }

        public void Dispose()
        {
            if (_state == -1)
                return;

            _current = default;
            _edgeEnumerator = default;
            _neighborVertex = default;
            _currentVertex = default;
            EnsureStackReleased();
            _state = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool UpdateColorMapAndCreateDiscoverVertexStep(TVertex vertex, int newState)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Gray);
            _current = new DfsStep<TVertex>(DfsStepKind.DiscoverVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool UpdateColorMapAndCreateFinishVertexStep(TVertex vertex, int newState)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Black);
            _current = new DfsStep<TVertex>(DfsStepKind.FinishVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Terminate()
        {
            _current = new DfsStep<TVertex>(DfsStepKind.None, _currentVertex);
            _state = -2;
            EnsureStackReleased();
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureStackInitialized() => _stack.Dispose();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureStackReleased() => _stack.Dispose();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushVertexStackFrame(TVertex vertex, TEdgeEnumerator edgeEnumerator) =>
            _stack.Add(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, false, default, edgeEnumerator));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PushEdgeStackFrame(TVertex vertex, TEdge edge, TEdgeEnumerator edgeEnumerator) =>
            _stack.Add(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, true, edge, edgeEnumerator));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame) =>
            _stack.TryTake(out stackFrame);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            _colorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
