using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.CommandLine.Utilities
{
   internal class CharReader : ILookaheadReader<int>
   {
      #region Private Fields

      private readonly string m_str;
      private int m_position;

      #endregion

      #region Construction

      public CharReader(string str)
      {
         m_str = str ?? String.Empty;
      }

      #endregion

      #region Public Methods

      public int Peek()
      {
         return Peek(0);
      }

      public int Peek(int offset)
      {
         if (m_position + offset < 0 || m_position + offset >= m_str.Length)
            return -1;

         return m_str[m_position + offset];
      }

      public int Read()
      {
         if (m_position < 0 || m_position >= m_str.Length)
            return -1;

         return m_str[m_position++];
      }

      #endregion
   }
}
