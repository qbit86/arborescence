namespace Ubiquitous
{
    using System.Collections.Generic;

    public struct ListFactory<TContext, TElem> : IFactory<TContext, List<TElem>>
    {
        public List<TElem> Acquire(TContext context)
        {
            return new List<TElem>();
        }

        public void Release(TContext context, List<TElem> value)
        {
            value?.Clear();
        }
    }
}
