using System;
using System.Collections.Generic;

namespace MailChimp.Net
{
    public static class ForEachEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> e, Action<T> a)
        {
            foreach (var v in e)
            {
                a(v);
            }
        }
    }
}
