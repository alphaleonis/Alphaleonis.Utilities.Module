using Alphaleonis.CommandLine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

            return Token.Value(sb.ToString());
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
         return new Token(TokenType.Value, sb.ToString(), TokenModifier.None);
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

         return new Token(TokenType.Value, sb.ToString(), TokenModifier.None);
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
   public class MyLexer : LexerBase
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

   public class WindowsStyleLexer : ILexer
   {
      private static readonly Regex s_optionRegex = new Regex(@"\A(?:-|/)(?<name>[^\s\:\=\-][^\s\:\=]*)(?:\s*(?<assign>=|:))?(?<value>.+)?", RegexOptions.Compiled);
      private static readonly Regex s_assignmentRegex = new Regex(@"\A(?<assign>=|:)(?<value>.+)?");

      //public IReadOnlyList<Token> Tokenize2(string[] args)
      //{
      //   List<Token> tokens = new List<Token>();
      //   if (args == null)
      //      return tokens;

      //   int i = 0;
      //   while (i < args.Length)
      //   {            
      //      var arg = args[i];
      //      if (arg == null || arg.Length == 0)
      //         continue;

      //      if (arg.Equals("--", StringComparison.Ordinal))
      //      {

      //      }
      //      if (arg[0] == '-' || arg[0] == '/')
      //      {
      //         MatchOption(tokens, arg);
      //      }
      //   }         
      //}


      private static int LA(string s, int i)
      {
         if (i >= s.Length)
            return -1;

         return s[i];
      }

      public IReadOnlyList<Token> Tokenize(string[] args)
      {
         List<Token> tokens = new List<Token>();
         bool wasTerminated = false;

         if (args != null)
         {
            foreach (var arg in args)
            {
               if (wasTerminated)
               {
                  tokens.Add(Token.Value(arg));
               }
               else if (arg.Equals("--") && (tokens.Count == 0 || tokens[tokens.Count - 1].TokenType != TokenType.Assignment))
               {
                  wasTerminated = true;
               }
               else
               {
                  Match optionMatch = s_optionRegex.Match(arg);
                  if (optionMatch.Success)
                  {
                     tokens.Add(Token.OptionName(optionMatch.Groups["name"].Value));

                     var assignmentGroup = optionMatch.Groups["assign"];
                     if (assignmentGroup.Success)
                        tokens.Add(Token.Assignment(optionMatch.Groups["assign"].Value));

                     var valueGroup = optionMatch.Groups["value"];
                     if (valueGroup.Success)
                     {
                        tokens.Add(Token.Value(valueGroup.Value));
                     }
                  }
                  else
                  {
                     Match assignmentMatch = s_assignmentRegex.Match(arg);
                     if (assignmentMatch.Success)
                     {
                        tokens.Add(Token.Assignment(assignmentMatch.Groups["assign"].Value));
                        var valueGroup = assignmentMatch.Groups["value"];
                        if (valueGroup.Success)
                           tokens.Add(Token.Value(valueGroup.Value));
                     }
                     else
                     {
                        tokens.Add(Token.Value(arg));
                     }
                  }
               }
            }
         }

         return tokens;

      }

      public string GetOptionDisplayText(IParameterDefinition parameter)
      {
         throw new NotImplementedException();
      }

      public void ValidateParameterNames(IParameterDefinition parameter)
      {
         throw new NotImplementedException();
      }
   }
}
