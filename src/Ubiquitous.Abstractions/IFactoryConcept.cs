namespace Ubiquitous
{
    public interface IFactoryConcept<TContext, TValue>
    {
        TValue Acquire(TContext graph);
        void Release(TContext graph, TValue value);
    }
}
