namespace Linn.Purchasing.Integration.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static T Second<T>(this IEnumerable<T> items)
        {
            return items.ElementAt(1);
        }
    }
}
