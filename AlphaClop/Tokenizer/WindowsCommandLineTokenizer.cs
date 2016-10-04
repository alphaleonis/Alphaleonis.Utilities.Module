using Alphaleonis.CommandLine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.CommandLine
{
   internal class WindowsCommandLineTokenizer : ICommandLineTokenizer
   {
      #region Public Methods

      public IReadOnlyList<Token> Tokenize(string commandLine)
      {
         List<Token> tokens = new List<Token>();
         CharReader reader = new CharReader(commandLine);
         TokenType lastReadTokenType = TokenType.None;
         Token next;
         while ((next = ReadNext(reader, lastReadTokenType)) != null)
         {
            lastReadTokenType = next.TokenType;
            tokens.Add(next);
         }

         return tokens;
      }      

      #endregion

      #region Private Methods

      private static Token ReadNext(CharReader reader, TokenType lastReadTokenType)
      {
         // Special handling of the first argument on the command line. This is normally the application name, but if using 
         // CreateProcess with both the applicationName and commandLine parameters, this may actually be an empty whitespace, in which 
         // case we return this as the first argument. (otherwise, whitespace is insignificant and we ignore it as below).
         if (lastReadTokenType == TokenType.None && reader.Peek() != -1 && Char.IsWhiteSpace((char)reader.Peek()))
         {
            StringBuilder sb = new StringBuilder();
            while (Char.IsWhiteSpace((char)reader.Peek()))
            {
               sb.Append(reader.Read());
            }

            return Token.UnqotedValue(sb.ToString());
         }

         // Skip any insignificant whitespace
         while (reader.Peek() != -1 && Char.IsWhiteSpace((char)reader.Peek()))
            reader.Read();

         // Check if we have more characters, otherwise there are no more tokens.
         if (reader.Peek() == -1)
            return null;

         if (reader.Peek() == '-' || reader.Peek() == '/')
            return ReadNextOptionName(reader);

         if (reader.Peek() == '=' || reader.Peek() == ':' && lastReadTokenType == TokenType.Name)
            return ReadNextAssignment(reader);

         if (reader.Peek() == '\"')
            return ReadNextQuotedValue(reader);

         return ReadNextUnquotedValue(reader);
      }

      private static Token ReadNextUnquotedValue(CharReader reader)
      {
         StringBuilder sb = new StringBuilder();
         while (reader.Peek() != -1 && !Char.IsWhiteSpace((char)reader.Peek()))
         {
            if (reader.Peek() == '\\')
               ReadEscapeCharacters(reader, sb);
            else
               sb.Append((char)reader.Read());
         }
         return new Token(TokenType.UnquotedValue, sb.ToString(), TokenModifier.None);
      }

      private static void ReadEscapeCharacters(CharReader reader, StringBuilder sb)
      {
         System.Diagnostics.Debug.Assert(reader.Peek() == '\\');

         int count = 0;
         while (reader.Peek() == '\\')
         {
            reader.Read();
            count++;
         }

         if (reader.Peek() == '\"')
         {
            reader.Read();
            sb.Append(new string('\\', count / 2));
            if ((count % 2) == 1)
            {
               sb.Append('\"');
            }
         }
         else
         {
            sb.Append(new string('\\', count));
         }
      }

      private static Token ReadNextQuotedValue(CharReader reader)
      {
         System.Diagnostics.Debug.Assert(reader.Peek() == '\"');

         StringBuilder sb = new StringBuilder();

         // Consume leading quote
         reader.Read();

         while (reader.Peek() != -1)
         {
            if (reader.Peek() == '\\')
            {
               ReadEscapeCharacters(reader, sb);
            }
            else if (reader.Peek() == '\"')
            {
               reader.Read();
               break;
            }
            else
            {
               sb.Append((char)reader.Read());
            }
         }

         return new Token(TokenType.QuotedValue, sb.ToString(), TokenModifier.None);
      }

      private static Token ReadNextAssignment(CharReader reader)
      {
         System.Diagnostics.Debug.Assert(reader.Peek() == '=' || reader.Peek() == ':');
         StringBuilder sb = new StringBuilder();
         sb.Append((char)reader.Read());
         return new Token(TokenType.Assignment, sb.ToString(), TokenModifier.None);
      }

      private static Token ReadNextOptionName(CharReader reader)
      {
         System.Diagnostics.Debug.Assert(reader.Peek() == '-' || reader.Peek() == '/');

         // Consume switch character
         reader.Read();

         StringBuilder sb = new StringBuilder();
         while (reader.Peek() != -1 && !Char.IsWhiteSpace((char)reader.Peek()) && reader.Peek() != '=' && reader.Peek() != ':')
         {
            sb.Append((char)reader.Read());
         }

         return new Token(TokenType.Name, sb.ToString(), TokenModifier.None);
      }

      #endregion
   }
}
