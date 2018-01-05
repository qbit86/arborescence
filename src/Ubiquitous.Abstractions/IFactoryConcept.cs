namespace Ubiquitous
{
    public interface IFactoryConcept<in TContext, TValue>
    {
        TValue Acquire(TContext context);
        void Release(TContext context, TValue value);
    }
}
