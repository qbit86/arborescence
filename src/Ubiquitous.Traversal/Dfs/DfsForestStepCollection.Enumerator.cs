namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
        TColorMap, TStack, TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
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

            private DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> _collection;
            private TColorMap _colorMap;
            private DisposalStatus _colorMapDisposalStatus;
            private TStack _stack;
            private DisposalStatus _stackDisposalStatus;
            private DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept> _stepEnumerator;

            public Enumerator(DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStack,
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
                            _state = 1;
                            continue;
                        }
                        case 1:
                        {
                            if (!_collection.VertexEnumerator.MoveNext())
                            {
                                _state = int.MaxValue;
                                continue;
                            }
                            _state = 2;
                            continue;
                        }
                        case 2:
                        {
                            Color vertexColor;
                            Assert(_colorMap != null, nameof(_colorMap) + " != null");
                            if (_colorMap.TryGetValue(_collection.VertexEnumerator.Current, out vertexColor))
                                vertexColor = Color.None;
                            if (vertexColor != Color.None && vertexColor != Color.White)
                            {
                                _state = 1;
                                continue;
                            }
                            _current = Step.Create(DfsStepKind.StartVertex, _collection.VertexEnumerator.Current,
                                default(TEdge));
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
                            throw new NotImplementedException();
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
                _colorMapDisposalStatus = DisposalStatus.None;
                _stackDisposalStatus = DisposalStatus.None;
                _current = default(Step<DfsStepKind, TVertex, TEdge>);
                _state = 0;
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

                _stepEnumerator.Dispose();
            }
        }
    }
}
