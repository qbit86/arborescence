namespace Ubiquitous.Traversal.Advanced
{
    public enum DfsStepKind
    {
        None = 0,
        StartVertex,
        DiscoverVertex,
        FinishVertex,
        ExamineEdge,
        TreeEdge,
        BackEdge,
        ForwardOrCrossEdge,
        FinishEdge,
    }
}
