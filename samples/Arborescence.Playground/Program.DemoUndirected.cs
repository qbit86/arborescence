namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Workbench;

    internal static partial class Program
    {
        private static void DemoUndirected()
        {
            UndirectedIndexedIncidenceGraph.Builder builder = new(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader("05"))
            {
                IEnumerable<Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints edge in edges)
                    builder.Add(edge.Tail, edge.Head);
            }

            UndirectedIndexedIncidenceGraph graph = builder.ToGraph();

            Console.Write($"{nameof(graph.VertexCount)}: {graph.VertexCount}");
            Console.WriteLine($", {nameof(graph.EdgeCount)}: {graph.EdgeCount}");

            if (graph.VertexCount <= 0)
                return;

            PrintIncidentEdges(graph, 0);

            if (graph.VertexCount == 1)
                return;

            PrintIncidentEdges(graph, graph.VertexCount - 1);

            static void PrintIncidentEdges(UndirectedIndexedIncidenceGraph graph, int vertex)
            {
                ArraySegment<int>.Enumerator edges = graph.EnumerateOutEdges(vertex);
                while (edges.MoveNext())
                {
                    int edge = edges.Current;
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
