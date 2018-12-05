namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    public struct JaggedAdjacencyListIncidenceGraphBuilder : IGraphBuilder<JaggedAdjacencyListIncidenceGraph, int, int>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
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

            _initialOutDegree = DefaultInitialOutDegree;
            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
            OutEdges = new ArrayPrefix<int>[vertexUpperBound];
        }

        public int VertexUpperBound => OutEdges?.Length ?? 0;

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        private ArrayPrefix<int>[] OutEdges { get; set; }

        public bool TryAdd(int source, int target, out int edge)
        {
            if (OutEdges == null)
            {
                edge = int.MinValue;
                return false;
            }

            if ((uint)source >= (uint)VertexUpperBound)
            {
                edge = -1;
                return false;
            }

            if ((uint)target >= (uint)VertexUpperBound)
            {
                edge = -2;
                return false;
            }

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            if (OutEdges[source].Array == null)
                OutEdges[source] = new ArrayPrefix<int>(ArrayPool<int>.Shared.Rent(InitialOutDegree), 0);

            ArrayPrefixBuilder.Add(ref OutEdges[source], newEdgeIndex);

            edge = newEdgeIndex;
            return true;
        }

        public JaggedAdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_sources.Count == _targets.Count);
            int[] targetsBuffer = _targets.Buffer ?? ArrayBuilder<int>.EmptyArray;
            int[] sourcesBuffer = _sources.Buffer ?? ArrayBuilder<int>.EmptyArray;
            int[] endpoints = _targets.Count > 0 ? new int[_targets.Count * 2] : ArrayBuilder<int>.EmptyArray;
            if (endpoints.Length > 0)
            {
                Array.Copy(targetsBuffer, 0, endpoints, 0, _targets.Count);
                Array.Copy(sourcesBuffer, 0, endpoints, _targets.Count, _sources.Count);
            }

            ArrayPrefix<int>[] outEdges = OutEdges ?? ArrayBuilder<ArrayPrefix<int>>.EmptyArray;

            _initialOutDegree = DefaultInitialOutDegree;
            _sources = default;
            _targets = default;
            OutEdges = null;

            return new JaggedAdjacencyListIncidenceGraph(endpoints, outEdges);
        }
    }
}
