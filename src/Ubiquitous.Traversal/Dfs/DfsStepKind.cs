namespace Ubiquitous.Traversal
{
    public enum DfsStepKind : byte
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
