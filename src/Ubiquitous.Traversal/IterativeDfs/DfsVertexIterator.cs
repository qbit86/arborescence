// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    internal struct DfsVertexIterator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;
        private TGraph _graph;
        private TColorMap _colorMap;

        internal DfsVertexStep<TVertex> _current;
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

            _current = new DfsVertexStep<TVertex>(DfsStepKind.None, startVertex);
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

            while (true)
            {
                switch (_state)
                {
                    case 1:
                        return CreateDiscoverVertexStep(_currentVertex, 2, out _current);
                    case 2:
                        EnsureStack();
                        TEdgeEnumerator edges = _graphPolicy.EnumerateOutEdges(_graph, _currentVertex);
                        PushVertexStackFrame(_currentVertex, edges);
                        _state = 3;
                        continue;
                    case 3:
                        if (!TryPopStackFrame(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame))
                            return Terminate(out _current);

                        _currentVertex = stackFrame.Vertex;
                        _edgeEnumerator = stackFrame.EdgeEnumerator;
                        // if (stackFrame.HasEdge)
                        //     return CreateEdgeStep(DfsStepKind.FinishEdge, stackFrame.Edge...);

                        _state = 4;
                        continue;
                    case 4:
                        if (!_edgeEnumerator.MoveNext())
                            return CreateFinishVertexStep(_currentVertex, 2, out _current);

                        bool isValid = _graphPolicy.TryGetTarget(_graph, _edgeEnumerator.Current, out _neighborVertex);
                        if (!isValid)
                            continue;

                        throw new NotImplementedException();
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        internal void Reset(TVertex startVertex)
        {
            Assert(_state > 0, "_state > 0");

            _current = new DfsVertexStep<TVertex>(DfsStepKind.None, startVertex);
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
        private bool CreateDiscoverVertexStep(TVertex vertex, int newState, out DfsVertexStep<TVertex> current)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Gray);
            current = new DfsVertexStep<TVertex>(DfsStepKind.DiscoverVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateFinishVertexStep(TVertex vertex, int newState, out DfsVertexStep<TVertex> current)
        {
            _colorMapPolicy.AddOrUpdate(_colorMap, _currentVertex, Color.Black);
            current = new DfsVertexStep<TVertex>(DfsStepKind.FinishVertex, vertex);
            _state = newState;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Terminate(out DfsVertexStep<TVertex> current)
        {
            current = new DfsVertexStep<TVertex>(DfsStepKind.None, _currentVertex);
            _state = -2;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureStack()
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
    }
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
