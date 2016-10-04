using System;
using System.Diagnostics;
using System.Linq;

namespace Alphaleonis.CommandLine
{
   public enum TokenModifier
   {
      None = 0,
      Include,
      Exclude
   }

   internal enum TokenType
   {
      None,      
      Name,
      Assignment,
      UnquotedValue,
      QuotedValue
   }


   [Serializable]
   [DebuggerDisplay("{TokenType} {Text}")]
   internal class Token : IEquatable<Token>
   {
      public static Token OptionName(string text, TokenModifier modifier = TokenModifier.None)
      {
         return new Token(TokenType.Name, text, modifier);
      }

      public static Token UnqotedValue(string text)
      {
         return new Token(TokenType.UnquotedValue, text, TokenModifier.None);
      }

      public static Token QuotedValue(string text)
      {
         return new Token(TokenType.QuotedValue, text, TokenModifier.None);
      }

      public static Token Assignment(string text)
      {
         return new Token(TokenType.Assignment, text, TokenModifier.None);
      }

      public Token(TokenType type, string text, TokenModifier modifier)
      {
         TokenType = type;
         Text = text;
         Modifier = modifier;
      }

      public TokenType TokenType { get; }

      public string Text { get; }

      public TokenModifier Modifier { get; }

      public bool Equals(Token other)
      {
         if (Object.ReferenceEquals(other, null))
            return false;

         return TokenType == other.TokenType && StringComparer.OrdinalIgnoreCase.Equals(Text, other.Text) && Modifier == other.Modifier;
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as Token);
      }

      public override int GetHashCode()
      {
         return TokenType.GetHashCode() ^ (11 * (Text?.GetHashCode() ?? 0));
      }

      public override string ToString()
      {
         return Text;
      }
   }
}
