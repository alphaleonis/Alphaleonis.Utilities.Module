using System;
using System.Linq;

namespace Alphaleonis.CommandLine.Utilities
{
   internal interface ILookaheadReader<T>
   {
      T Peek();
      T Peek(int offset);
      T Read();
   }
}
