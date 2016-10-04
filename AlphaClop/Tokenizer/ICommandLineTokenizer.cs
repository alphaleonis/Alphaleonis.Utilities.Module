using System.Collections;
using System.Collections.Generic;

namespace Alphaleonis.CommandLine
{
   internal interface ICommandLineTokenizer
   {
      IReadOnlyList<Token> Tokenize(string commandLine);
   }
}