namespace Ubiquitous
{
    public enum DfsStepKind
    {
        None = 0,
        StartVertex,
        DiscoverVertex,
        ExamineVertex,
        FinishVertex,
        ExamineEdge,
        TreeEdge,
        BackEdge,
        ForwardOrCrossEdge,
        FinishEdge,
    }
}
