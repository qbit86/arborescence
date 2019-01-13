// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Internal;
    using static System.Diagnostics.Debug;

    public readonly partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
        TGraphPolicy, TColorMapPolicy, TStepPolicy>
    {
        public struct Enumerator : IEnumerator<TStep>
        {
            private TStep _current;
            private int _state;

            private TColorMapPolicy _colorMapPolicy;
            private TGraphPolicy _graphPolicy;
            private TStepPolicy _stepPolicy;

            private readonly TGraph _graph;
            private readonly TVertex _startVertex;
            private readonly int _stackCapacity;
            private TColorMap _colorMap;
            private DisposalStatus _colorMapDisposalStatus;
            private List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> _stack;
            private DisposalStatus _stackDisposalStatus;

            private DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy> _stepEnumerator;

            internal Enumerator(in DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy> collection)
            {
                Assert(collection.GraphPolicy != null);
                Assert(collection.ColorMapPolicy != null);
                Assert(collection.StepPolicy != null);

                _current = default;
                _state = 0;

                _graph = collection.Graph;
                _startVertex = collection.StartVertex;
                _stackCapacity = collection.StackCapacity;
                _graphPolicy = collection.GraphPolicy;
                _colorMapPolicy = collection.ColorMapPolicy;
                _stepPolicy = collection.StepPolicy;
                _colorMap = default;
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = null;
                _stackDisposalStatus = DisposalStatus.None;
                _stepEnumerator = default;
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                        {
                            _colorMap = _colorMapPolicy.Acquire();
                            if (_colorMap == null)
                                return Terminate();

                            _colorMapDisposalStatus = DisposalStatus.Initialized;
                            _state = 2;
                            continue;
                        }
                        case 2:
                        {
                            _current = _stepPolicy.CreateVertexStep(DfsStepKind.StartVertex, _startVertex);
                            _state = 3;
                            return true;
                        }
                        case 3:
                        {
                            _stack = ListCache<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Acquire(_stackCapacity);
                            if (_stack == null)
                                return Terminate();

                            _stackDisposalStatus = DisposalStatus.Initialized;
                            _state = 4;
                            continue;
                        }
                        case 4:
                        {
                            ThrowIfDisposed();
                            _stepEnumerator = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                                TStep, TGraphPolicy, TColorMapPolicy, TStepPolicy>(_graph, _startVertex, _colorMap,
                                _stack, _graphPolicy, _colorMapPolicy, _stepPolicy);
                            _state = 5;
                            continue;
                        }
                        case 5:
                        {
                            ThrowIfDisposed();
                            if (!_stepEnumerator.MoveNext())
                                return Terminate();

                            _current = _stepEnumerator.Current;
                            _state = 5;
                            return true;
                        }
                    }

                    return Terminate();
                }
            }

            public void Reset()
            {
                DisposeCore();

                _current = default;
                _state = 0;

                _colorMap = default;
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = null;
                _stackDisposalStatus = DisposalStatus.None;
                _stepEnumerator = default;
            }

            // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
            public TStep Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
                DisposeCore();
                _current = default;
                _state = -1;
            }

            private void DisposeCore()
            {
                if (_colorMapDisposalStatus == DisposalStatus.Initialized)
                {
                    _colorMapPolicy.Release(_colorMap);
                    _colorMap = default;
                    _colorMapDisposalStatus = DisposalStatus.Disposed;
                }

                if (_stackDisposalStatus == DisposalStatus.Initialized)
                {
                    ListCache<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Release(_stack);
                    _stack = null;
                    _stackDisposalStatus = DisposalStatus.Disposed;
                }
            }

            private void ThrowIfDisposed()
            {
                if (_colorMapDisposalStatus != DisposalStatus.Initialized)
                    throw new ObjectDisposedException(nameof(_colorMap));
                if (_stackDisposalStatus != DisposalStatus.Initialized)
                    throw new ObjectDisposedException(nameof(_stack));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool Terminate()
            {
                DisposeCore();
                _current = default;
                _state = -1;
                return false;
            }
        }
    }
}
