namespace Arborescence
{
    using System.Collections.Generic;

    internal readonly struct QueuePolicy : IContainerPolicy<Queue<int>, int>
    {
        public void Add(Queue<int> container, int item) => container.Enqueue(item);

        public bool TryTake(Queue<int> container, out int result) => container.TryDequeue(out result);
    }
}
