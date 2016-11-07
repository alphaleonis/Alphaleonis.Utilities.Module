using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Utilities
{
   public static class EnumerableExtensions
   {
      public static bool HasDuplicates<T>(this IEnumerable<T> self)
      {
         return HasDuplicates<T>(self, null);
      }

      public static bool HasDuplicates<T>(this IEnumerable<T> self, IEqualityComparer<T> comparer)
      {
         if (self == null)
            throw new ArgumentNullException(nameof(self), $"{nameof(self)} is null.");

         if (comparer == null)
            comparer = EqualityComparer<T>.Default;

         return self.GroupBy(x => x, comparer).Any(g => g.Count() > 1);
      }
   }
}
