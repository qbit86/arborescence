namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public sealed class JaggedAdjacencyListIncidenceGraphBuilder
    {
        private ArrayBuilder<int> _sources;
        private ArrayBuilder<int> _targets;

        public JaggedAdjacencyListIncidenceGraphBuilder(int vertexUpperBound) : this(vertexUpperBound, 0)
        {
        }

        public JaggedAdjacencyListIncidenceGraphBuilder(int vertexUpperBound, int edgeCount)
        {
            if (vertexUpperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexUpperBound));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            OutEdges = new ArrayBuilder<int>[vertexUpperBound];
            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
        }

        public int VertexUpperBound => OutEdges?.Length ?? 0;

        private ArrayBuilder<int>[] OutEdges { get; set; }

        public int Add(int source, int target)
        {
            if (OutEdges == null)
                return -1;

            if ((uint)source >= (uint)VertexUpperBound)
                return -1;

            if ((uint)target >= (uint)VertexUpperBound)
                return -1;

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            if (OutEdges[source].Buffer == null)
                OutEdges[source] = new ArrayBuilder<int>(1);

            OutEdges[source].Add(newEdgeIndex);

            return newEdgeIndex;
        }

        public JaggedAdjacencyListIncidenceGraph MoveToIndexedAdjacencyListGraph()
        {
            Assert(_sources.Count == _targets.Count);
            int[] endpoints = _targets.Count > 0 ? new int[_targets.Count * 2] : ArrayBuilder<int>.EmptyArray;
            if (endpoints.Length > 0)
            {
                Array.Copy(_targets.Buffer, 0, endpoints, 0, _targets.Count);
                Array.Copy(_sources.Buffer, 0, endpoints, _targets.Count, _sources.Count);
            }

            _sources = default;
            _targets = default;

            ArrayBuilder<int>[] outEdges = OutEdges ?? ArrayBuilder<ArrayBuilder<int>>.EmptyArray;
            OutEdges = null;

            return new JaggedAdjacencyListIncidenceGraph(endpoints, outEdges);
        }
    }
}
