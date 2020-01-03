namespace Ubiquitous
{
    public interface IFactory<TValue>
    {
        TValue Acquire();
        void Release(TValue value);
    }
}
