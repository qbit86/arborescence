namespace Ubiquitous.Internal
{
    using System;
    using System.Collections.Generic;

    internal abstract class ListPool<T>
    {
        [ThreadStatic] private static ListPool<T> s_sharedInstance;

        public static ListPool<T> Shared => s_sharedInstance ?? (s_sharedInstance = new DefaultListPool<T>());

        public abstract List<T> Rent(int desiredCapacity);

        public abstract void Return(List<T> list);
    }

    internal sealed class DefaultListPool<T> : ListPool<T>
    {
        private WeakReference<List<T>> _cachedInstanceReference;

        private WeakReference<List<T>> CachedInstanceReference => _cachedInstanceReference
            ?? (_cachedInstanceReference = new WeakReference<List<T>>(null, false));

        public override List<T> Rent(int desiredCapacity)
        {
            bool isCached = CachedInstanceReference.TryGetTarget(out List<T> result);
            CachedInstanceReference.SetTarget(null);

            if (desiredCapacity > 0)
            {
                if (isCached)
                {
                    if (result.Capacity < desiredCapacity)
                        result.Capacity = desiredCapacity;

                    return result;
                }

                return new List<T>(desiredCapacity);
            }

            return isCached ? result : new List<T>();
        }

        public override void Return(List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            list.Clear();
            CachedInstanceReference.SetTarget(list);
        }
    }
}
