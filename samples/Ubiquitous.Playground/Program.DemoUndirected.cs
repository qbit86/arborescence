namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Workbench;

    internal static partial class Program
    {
        // ReSharper disable once UnusedMember.Local
        private static void DemoUndirected()
        {
            var builder = new UndirectedAdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("05"))
            {
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            UndirectedAdjacencyListIncidenceGraph graph = builder.ToGraph();

            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount}");

            if (graph.VertexCount <= 0)
                return;

            PrintIncidentEdges(graph, 0);

            if (graph.VertexCount == 1)
                return;

            PrintIncidentEdges(graph, graph.VertexCount - 1);

            static void PrintIncidentEdges(UndirectedAdjacencyListIncidenceGraph graph, int vertex)
            {
                ArraySegmentEnumerator<int> edges = graph.EnumerateOutEdges(vertex);
                foreach (int edge in edges)
                {
                    string tailString = graph.TryGetTail(edge, out int tail) ? IndexToString(tail) : "?";
                    string headString = graph.TryGetHead(edge, out int head) ? IndexToString(head) : "?";
                    string endpointsString = $"{tailString} -> {headString}";
                    Console.WriteLine($"{edge.ToString(CultureInfo.InvariantCulture)}: {endpointsString}");
                }
            }

            static string IndexToString(int i) => Base32.ToString(i);
        }
    }
}
