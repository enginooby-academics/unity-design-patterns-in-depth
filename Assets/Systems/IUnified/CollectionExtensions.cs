using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.IUnified
{
    public static class CollectionExtensions
    {
        public static int FirstIndexWhere<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                if(predicate(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static List<TContainer> ToContainerList<TContainer, TInterface>(this IEnumerable<TInterface> interfaces)
            where TInterface : class
            where TContainer : IUnifiedContainer<TInterface>, new()
        {
            return interfaces == null ? null : interfaces.Select(i => new TContainer { Result = i }).ToList();
        }
    }
}