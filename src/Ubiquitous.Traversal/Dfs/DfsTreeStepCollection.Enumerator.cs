namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
    {
        public struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private enum DisposalStatus
            {
                None = 0,
                Initialized,
                Disposed,
            }

            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;

            private DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> _collection;
            private TColorMap _colorMap;
            private DisposalStatus _colorMapDisposalStatus;
            private TStack _stack;
            private DisposalStatus _stackDisposalStatus;
            private DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept> _stepEnumerator;

            internal Enumerator(DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> collection)
            {
                Assert(collection.ColorMapFactory != null);
                Assert(collection.StackFactory != null);

                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _collection = collection;
                _colorMap = default(TColorMap);
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = default(TStack);
                _stackDisposalStatus = DisposalStatus.None;
                _stepEnumerator = default(DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator,
                    TColorMap, TStack, TVertexConcept, TEdgeConcept>);
            }

            public bool MoveNext()
            {
                while (true)
                {
                    switch (_state)
                    {
                        case 0:
                        {
                            _colorMap = _collection.ColorMapFactory.Acquire(_collection.Graph);
                            if (_colorMap == null)
                            {
                                _state = int.MaxValue;
                                continue;
                            }
                            _colorMapDisposalStatus = DisposalStatus.Initialized;
                            _state = 2;
                            continue;
                        }
                        case 2:
                        {
                            _current = Step.Create(DfsStepKind.StartVertex, _collection.StartVertex, default(TEdge));
                            _state = 3;
                            return true;
                        }
                        case 3:
                        {
                            _stack = _collection.StackFactory.Acquire(_collection.Graph);
                            if (_stack == null)
                            {
                                _state = int.MaxValue;
                                continue;
                            }
                            _stackDisposalStatus = DisposalStatus.Initialized;
                            _state = 4;
                            continue;
                        }
                        case 4:
                        {
                            ThrowIfDisposed();
                            _stepEnumerator = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator,
                                TColorMap, TStack, TVertexConcept, TEdgeConcept>(
                                _collection.Graph, _collection.StartVertex, _colorMap, _stack,
                                _collection.VertexConcept, _collection.EdgeConcept);
                            _state = 5;
                            continue;
                        }
                        case 5:
                        {
                            ThrowIfDisposed();
                            if (!_stepEnumerator.MoveNext())
                            {
                                _state = int.MaxValue;
                                continue;
                            }
                            _current = _stepEnumerator.Current;
                            _state = 5;
                            return true;
                        }
                        case int.MaxValue:
                        {
                            DisposeCore();
                            _current = default(Step<DfsStepKind, TVertex, TEdge>);
                            _state = -1;
                            return false;
                        }
                    }

                    return false;
                }
            }

            public void Reset()
            {
                DisposeCore();

                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;

                _colorMap = default(TColorMap);
                _colorMapDisposalStatus = DisposalStatus.None;
                _stack = default(TStack);
                _stackDisposalStatus = DisposalStatus.None;
                _stepEnumerator = default(DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator,
                    TColorMap, TStack, TVertexConcept, TEdgeConcept>);
            }

            public Step<DfsStepKind, TVertex, TEdge> Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
                DisposeCore();
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = -1;
            }

            private void DisposeCore()
            {
                if (_colorMapDisposalStatus == DisposalStatus.Initialized)
                {
                    _collection.ColorMapFactory.Release(_collection.Graph, _colorMap);
                    _colorMap = default(TColorMap);
                    _colorMapDisposalStatus = DisposalStatus.Disposed;
                }

                if (_stackDisposalStatus == DisposalStatus.Initialized)
                {
                    _collection.StackFactory.Release(_collection.Graph, _stack);
                    _stack = default(TStack);
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
        }
    }
}
