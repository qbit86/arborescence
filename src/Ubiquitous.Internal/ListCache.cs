namespace Ubiquitous.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class ListCache<T>
    {
        [ThreadStatic] private static WeakReference<List<T>> s_cachedInstanceReference;

        private static WeakReference<List<T>> CachedInstanceReference => s_cachedInstanceReference
            ?? (s_cachedInstanceReference = new WeakReference<List<T>>(null, false));

        public static List<T> Acquire(int desiredCapacity = 0)
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

        public static void Release(List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            list.Clear();
            CachedInstanceReference.SetTarget(list);
        }
    }
}
