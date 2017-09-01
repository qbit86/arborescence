namespace Ubiquitous
{
    public enum StepKind
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
        NonTreeEdge,
        FinishEdge,
    }
}
