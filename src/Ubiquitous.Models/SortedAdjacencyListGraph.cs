namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public readonly struct SortedAdjacencyListGraph : IEquatable<SortedAdjacencyListGraph>
    {
        private ArrayPrefix<SourceTargetPair<int>> Endpoints { get; }

        internal SortedAdjacencyListGraph(ArrayPrefix<SourceTargetPair<int>> endpoints)
        {
            Assert(endpoints.Array != null);

            Endpoints = endpoints;
        }

        public bool TryGetEndpoints(int edge, out SourceTargetPair<int> endpoints)
        {
            if ((uint)edge >= (uint)Endpoints.Count)
            {
                endpoints = default;
                return false;
            }

            endpoints = Endpoints.Array[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out ArrayPrefixEnumerator<int> outEdges)
        {
            throw new NotImplementedException();
        }

        public bool Equals(SortedAdjacencyListGraph other)
        {
            if (!Endpoints.Equals(other.Endpoints))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is SortedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Endpoints.GetHashCode();
        }
    }
}
