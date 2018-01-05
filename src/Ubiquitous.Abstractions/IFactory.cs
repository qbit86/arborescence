namespace Ubiquitous
{
    public interface IFactory<in TContext, TValue>
    {
        TValue Acquire(TContext context);
        void Release(TContext context, TValue value);
    }
}
