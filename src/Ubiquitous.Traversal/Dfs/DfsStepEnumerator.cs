﻿namespace Ubiquitous
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    // http://www.boost.org/doc/libs/1_65_1/boost/graph/depth_first_search.hpp
    internal struct DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept>

        : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
    {
        private Step<DfsStepKind, TVertex, TEdge> _current;
        private int _state;

        private TEdgeEnumerator _edgeEnumerator; // Corresponds to iterator range in Boost implementation.
        private TVertex _neighborVertex; // Corresponds to `v` in Boost implementation.
        private TVertex _currentVertex; // Corresponds to `u` in Boost implementation.

        private TGraph Graph { get; }

        private TColorMap ColorMap { get; }

        private TStack Stack { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal DfsStepEnumerator(TGraph graph, TVertex startVertex, TColorMap colorMap, TStack stack,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(colorMap != null);
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            ColorMap = colorMap;
            Stack = stack;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;

            _current = default(Step<DfsStepKind, TVertex, TEdge>);
            _state = 0;

            _edgeEnumerator = default(TEdgeEnumerator);
            _neighborVertex = default(TVertex);
            _currentVertex = startVertex;
        }

        public Step<DfsStepKind, TVertex, TEdge> Current => _current;

        object System.Collections.IEnumerator.Current => _current;

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
                        ColorMap[_currentVertex] = Color.Gray;
                        _current = Step.Create(DfsStepKind.DiscoverVertex, _currentVertex, default(TEdge));
                        _state = 1;
                        return true;
                    }
                case 1:
                    {
                        TEdgeEnumerator edges;
                        bool hasOutEdges = VertexConcept.TryGetOutEdges(Graph, _currentVertex, out edges);
                        if (!hasOutEdges)
                        {
                            ColorMap[_currentVertex] = Color.Black;
                            _current = Step.Create(DfsStepKind.FinishVertex, _currentVertex, default(TEdge));
                            _state = int.MaxValue;
                            return true;
                        }
                        var pushingStackFrame = CreateVertexStackFrame(_currentVertex, edges);
                        Stack.Add(pushingStackFrame);
                        _state = 2;
                        continue;
                    }
                case 2:
                    {
                        if (Stack.Count <= 0)
                        {
                            _state = int.MaxValue;
                            continue;
                        }
                        DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> poppedStackFrame = Stack[Stack.Count - 1];
                        Stack.RemoveAt(Stack.Count - 1);
                        _currentVertex = poppedStackFrame.Vertex;
                        _edgeEnumerator = poppedStackFrame.EdgeEnumerator;
                        if (poppedStackFrame.HasEdge)
                        {
                            _current = Step.Create(DfsStepKind.FinishEdge, default(TVertex), poppedStackFrame.Edge);
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
                        bool isValid = EdgeConcept.TryGetTarget(Graph, _edgeEnumerator.Current, out _neighborVertex);
                        if (!isValid)
                        {
                            _state = 3;
                            continue;
                        }
                        _current = Step.Create(DfsStepKind.ExamineEdge, default(TVertex), _edgeEnumerator.Current);
                        _state = 4;
                        return true;
                    }
                case 4:
                    {
                        Color neighborColor;
                        if (!ColorMap.TryGetValue(_neighborVertex, out neighborColor))
                            neighborColor = Color.None;
                        TEdge edge = _edgeEnumerator.Current;
                        switch (neighborColor)
                        {
                        case Color.None:
                        case Color.White:
                            _current = Step.Create(DfsStepKind.TreeEdge, default(TVertex), edge);
                            _state = 5;
                            return true;
                        case Color.Gray:
                            _current = Step.Create(DfsStepKind.BackEdge, default(TVertex), edge);
                            _state = 7;
                            return true;
                        default:
                            _current = Step.Create(DfsStepKind.ForwardOrCrossEdge, default(TVertex), edge);
                            _state = 7;
                            return true;
                        }
                    }
                case 5:
                    {
                        var pushingStackFrame = CreateEdgeStackFrame(_currentVertex, _edgeEnumerator.Current, _edgeEnumerator);
                        Stack.Add(pushingStackFrame);
                        _currentVertex = _neighborVertex;
                        ColorMap[_currentVertex] = Color.Gray;
                        _current = Step.Create(DfsStepKind.DiscoverVertex, _currentVertex, default(TEdge));
                        _state = 6;
                        return true;
                    }
                case 6:
                    {
                        bool hasOutEdges = VertexConcept.TryGetOutEdges(Graph, _currentVertex, out _edgeEnumerator);
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
                        _current = Step.Create(DfsStepKind.FinishEdge, default(TVertex), _edgeEnumerator.Current);
                        _state = 3;
                        return true;
                    }
                case short.MaxValue:
                    {
                        ColorMap[_currentVertex] = Color.Black;
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
                        string message = $"Enumerator is in unexpected state {_state}";
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
