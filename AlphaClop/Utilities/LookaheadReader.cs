using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.CommandLine.Utilities
{   
   internal class LookaheadReader<T> : ILookaheadReader<T>
   {
      private readonly IReadOnlyList<T> m_items;
      private int m_position;

      public LookaheadReader(IReadOnlyList<T> items)
      {
         m_items = items;
      }

      public T Peek()
      {         
         return Peek(0);
      }

      public T Peek(int offset)
      {
         if (m_position + offset < 0 || m_position + offset >= m_items.Count)
            return default(T);

         return m_items[m_position + offset];
      }

      public T Read()
      {
         if (m_position < 0 || m_position >= m_items.Count)
            return default(T);

         return m_items[m_position++];
      }

   }
}
