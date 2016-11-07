using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.CommandLine
{
   // TODO: Complete documentation
   // <parameterOrArgument> :=
   //                        <dash><parameterName>  <space>+ <argumentValue> |
   //                        <dash><parameterName> (':' | '=') <argumentValue> |
   //                        <dash><parameterName> |
   //                        <argumentValue>
   // <dash> := '/' | '-'


   public abstract class LexerBase
   {
      protected class Context
      {
         private readonly string[] m_args;
         private int m_currentArgIndex = -1;
         private int m_currentCharPos;

         public Context(string[] args)
         {
            if (args == null)
               throw new ArgumentNullException(nameof(args));

            m_args = args;
         }

         public string CurrentArg
         {
            get
            {
               if (m_currentArgIndex == -1)
                  throw new InvalidOperationException("Before first argument.");

               if (m_currentArgIndex >= m_args.Length)
                  return null;

               return m_args[m_currentArgIndex];
            }
         }

         public List<Token> Tokens { get; } = new List<Token>();

         public int Peek(int offset)
         {
            if (m_currentArgIndex < m_args.Length && (offset + m_currentCharPos) < m_args[m_currentArgIndex].Length)
            {
               return m_args[m_currentArgIndex][offset + m_currentCharPos];
            }

            return -1;
         }

         public int Read()
         {
            int next = Peek(0);
            if (next != -1)
               m_currentCharPos++;
            return next;
         }

         public bool MoveNextArg()
         {
            if (m_currentArgIndex < m_args.Length)
            {
               m_currentArgIndex++;
               m_currentCharPos = 0;
            }

            return m_currentArgIndex < m_args.Length;
         }

         public bool OptionsTerminated { get; set; }
      }

      // TODO: Needs parameter set definition as well.
      public IReadOnlyList<Token> Tokenize(string[] args)
      {
         Context context = new Context(args);
         Tokenize(context);
         return context.Tokens;
      }

      // TODO: Needs parameter set definition as well.
      protected abstract void Tokenize(Context context);
   }
}
