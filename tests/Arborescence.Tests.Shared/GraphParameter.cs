namespace Arborescence
{
    internal static class GraphParameter
    {
        internal static GraphParameter<TGraph> Create<TGraph>(TGraph graph, string description)
        {
            return new GraphParameter<TGraph>(graph, description);
        }
    }

    internal sealed class GraphParameter<TGraph>
    {
        private readonly string _description;

        internal GraphParameter(TGraph graph, string description)
        {
            _description = description ?? string.Empty;
            Graph = graph;
        }

        internal TGraph Graph { get; }

        public override string ToString() => _description;
    }
}
