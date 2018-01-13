using System.Collections;

namespace Ubiquitous
{
    using System.Collections.Generic;

    internal partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>
    {
        public struct Enumerator : IEnumerator<Step<DfsStepKind, TVertex, TEdge>>
        {
            private Step<DfsStepKind, TVertex, TEdge> _current;
            private int _state;
            private DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
                TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory> _collection;

            public bool MoveNext()
            {
                throw new System.NotImplementedException();
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public Step<DfsStepKind, TVertex, TEdge> Current { get; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
