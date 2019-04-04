namespace Ubiquitous.Traversal
{
    // “Forward and cross edges never occur in a depth-first search of an undirected graph.”
    // --- CLRS

#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum DfsStepKind : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        None = 0,
        StartVertex,
        DiscoverVertex,
        FinishVertex,
        ExamineEdge,
        FinishEdge,
        TreeEdge,
        BackEdge,
        ForwardOrCrossEdge
    }
}
