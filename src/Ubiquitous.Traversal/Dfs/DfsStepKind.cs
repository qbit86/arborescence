namespace Ubiquitous.Traversal
{
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum DfsStepKind : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        None = 0,
        StartVertex,
        DiscoverVertex,
        FinishVertex,
        ExamineEdge,
        TreeEdge,
        BackEdge,
        ForwardOrCrossEdge,
        FinishEdge
    }
}
