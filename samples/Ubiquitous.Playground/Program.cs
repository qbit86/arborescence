namespace Ubiquitous.Playground
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using Models;

    internal static class Program
    {
        private static TextReader In { get; } = CreateInputReader();

        private static TextWriter Out => Console.Out;

        private static void Main()
        {
            string header = In.ReadLine() ?? string.Empty;
            string[] parts = header.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);

            int vertexCount = Convert.ToInt32(parts[0], CultureInfo.InvariantCulture);
            int edgeCount = Convert.ToInt32(parts[1], CultureInfo.InvariantCulture);

            var graphBuilder = new AdjacencyListIncidenceGraphBuilder(vertexCount, edgeCount);
            IEnumerable<SourceTargetPair<int>> edges = IndexedEdgeListParser.ParseEdges(In);
            foreach (SourceTargetPair<int> edge in edges)
            {
                graphBuilder.TryAdd(edge.Source, edge.Target, out int _);
                graphBuilder.TryAdd(edge.Target, edge.Source, out int _);
            }

            AdjacencyListIncidenceGraph graph = graphBuilder.ToGraph();

            Out.WriteLine(graph.EdgeCount);
        }

        private static TextReader CreateInputReader()
        {
            return new StringReader("4 2\n1 2\n3 2");
        }
    }

    internal static class IndexedEdgeListParser
    {
        public static IEnumerable<SourceTargetPair<int>> ParseEdges(TextReader textReader)
        {
            if (textReader == null)
                throw new ArgumentNullException(nameof(textReader));

            return ParseEdgesCore(textReader);
        }

        private static IEnumerable<SourceTargetPair<int>> ParseEdgesCore(TextReader textReader)
        {
            Debug.Assert(textReader != null);

            for (string line = textReader.ReadLine(); line != null; line = textReader.ReadLine())
            {
                string[] parts = line.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    continue;

                int source;
                if (!int.TryParse(parts[0], out source))
                    continue;

                int target;
                if (!int.TryParse(parts[1], out target))
                    continue;

                yield return SourceTargetPair.Create(source - 1, target - 1);
            }
        }
    }
}
