namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public sealed class JaggedAdjacencyListGraphBuilder
    {
        private ArrayBuilder<int> _sources;
        private ArrayBuilder<int> _targets;

        public JaggedAdjacencyListGraphBuilder(int vertexCount) : this(vertexCount, 0)
        {
        }

        public JaggedAdjacencyListGraphBuilder(int vertexCount, int edgeCount)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            OutEdges = new ArrayBuilder<int>[vertexCount];
            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
        }

        private ArrayBuilder<int>[] OutEdges { get; set; }

        public int VertexCount => OutEdges?.Length ?? 0;

        public int Add(int source, int target)
        {
            if (OutEdges == null)
                return -1;

            if ((uint)source >= (uint)VertexCount)
                return -1;

            if ((uint)target >= (uint)VertexCount)
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

        public JaggedAdjacencyListGraph MoveToIndexedAdjacencyListGraph()
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

            ArrayBuilder<int>[] outEdges = OutEdges ?? new ArrayBuilder<int>[0];
            OutEdges = null;

            return new JaggedAdjacencyListGraph(endpoints, outEdges);
        }
    }
}
