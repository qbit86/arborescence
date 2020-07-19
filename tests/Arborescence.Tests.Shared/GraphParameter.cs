namespace Arborescence
{
    using Models;

    internal sealed class GraphParameter
    {
        private readonly string _description;

        internal GraphParameter(IndexedIncidenceGraph graph, string description)
        {
            _description = description ?? string.Empty;
            Graph = graph;
        }

        internal IndexedIncidenceGraph Graph { get; }

        public override string ToString() => _description;
    }
}
