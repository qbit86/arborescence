namespace Ubiquitous
{
    using Models;

    internal sealed class GraphParameter
    {
        private readonly string _description;

        internal GraphParameter(AdjacencyListIncidenceGraph graph, string description)
        {
            _description = description ?? string.Empty;
            Graph = graph;
        }

        internal AdjacencyListIncidenceGraph Graph { get; }

        public override string ToString() => _description;
    }
}
