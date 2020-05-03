namespace Ubiquitous
{
    public interface IMonoidPolicy<T>
    {
        T Identity { get; }
        T Combine(T left, T right);
    }
}