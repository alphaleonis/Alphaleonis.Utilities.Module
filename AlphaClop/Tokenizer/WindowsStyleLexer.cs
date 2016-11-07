using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Alphaleonis.CommandLine
{
   public class WindowsStyleLexer : LexerBase
   {
      protected override void Tokenize(Context context)
      {
         while (context.MoveNextArg())
         {
            Console.WriteLine($"Parsing: \"{context.CurrentArg}\"");
            if (context.OptionsTerminated)
               MatchValue(context);
            else
               MatchOptionNameOrValue(context);
         }
      }

      private void MatchOptionNameOrValue(Context context)
      {
         if (context.CurrentArg.Equals("--", StringComparison.Ordinal))
         {
            context.OptionsTerminated = true;
         }
         else if (!context.OptionsTerminated && (context.Peek(0) == '-' || context.Peek(0) == '/') && context.Peek(1) != -1 && !Char.IsWhiteSpace((char)context.Peek(1)))
         {
            MatchOptionName(context);
         }
         else
         {
            MatchValue(context);
         }
      }

      private void MatchOptionName(Context context)
      {
         Contract.Assert(context.Peek(0) == '-' || context.Peek(0) == '/');

         StringBuilder sb = new StringBuilder();
         context.Read(); // Consume the switch character.

         int ch = context.Peek(0);
         while (ch != -1 && ch != ':' && ch != '=')
         {
            sb.Append((char)context.Read());
            ch = context.Peek(0);
         }

         context.Tokens.Add(Token.OptionName(sb.ToString()));

         if (ch != -1)
         {
            context.Tokens.Add(Token.Assignment(((char)context.Read()).ToString()));
            MatchValue(context);
         }
      }

      private void MatchValue(Context context)
      {
         StringBuilder sb = new StringBuilder();
         int ch;
         while ((ch = context.Read()) != -1)
         {
            sb.Append((char)ch);
         }

         context.Tokens.Add(Token.Value(sb.ToString()));
      }
   }
}
