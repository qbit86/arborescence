namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using static System.Diagnostics.Debug;

    internal static partial class Program
    {
        private static void SerializeGraphByEdges(IndexedAdjacencyListGraph graph,
            IReadOnlyDictionary<int, DfsStepKind> vertexKinds, IReadOnlyDictionary<int, DfsStepKind> edgeKinds,
            string graphName, TextWriter textWriter)
        {
            Assert(graphName != null);
            Assert(textWriter != null);

            textWriter.WriteLine($"digraph \"{graphName}\"{Environment.NewLine}{{");
            try
            {
                textWriter.WriteLine("    node [shape=circle]");
                if (vertexKinds != null)
                {
                    for (int v = 0; v < graph.VertexCount; ++v)
                    {
                        DfsStepKind vertexKind;
                        if (!vertexKinds.TryGetValue(v, out vertexKind))
                            continue;

                        if (vertexKind == DfsStepKind.StartVertex)
                            textWriter.WriteLine($"    {v} [style=filled]");
                    }
                }

                for (int e = 0; e < graph.EdgeCount; ++e)
                {
                    SourceTargetPair<int> endpoints;
                    if (!graph.TryGetEndpoints(e, out endpoints))
                        continue;

                    textWriter.Write($"    {endpoints.Source} -> {endpoints.Target}");

                    DfsStepKind edgeKind;
                    if (edgeKinds == null || !edgeKinds.TryGetValue(e, out edgeKind))
                    {
                        textWriter.WriteLine();
                        continue;
                    }

                    // http://www.graphviz.org/Documentation/dotguide.pdf
                    switch (edgeKind)
                    {
                        case DfsStepKind.TreeEdge:
                            textWriter.WriteLine(" [style=bold]");
                            continue;
                        case DfsStepKind.BackEdge:
                            textWriter.WriteLine(" [style=dotted]");
                            continue;
                        case DfsStepKind.ForwardOrCrossEdge:
                            textWriter.WriteLine(" [style=dashed]");
                            continue;
                    }

                    textWriter.WriteLine(" [style=dotted]");
                }
            }
            finally
            {
                textWriter.WriteLine("}");
            }
        }
    }
}
