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

   public enum TokenType
   {
      None,
      Name,
      Assignment,
      Value
   }


   [Serializable]
   [DebuggerDisplay("{TokenType} {Text}")]
   public class Token : IEquatable<Token>
   {
      public static readonly Token Empty = new Token(TokenType.None, "", TokenModifier.None);
       
      public static Token OptionName(string text, TokenModifier modifier = TokenModifier.None)
      {
         return new Token(TokenType.Name, text, modifier);
      }

      public static Token Value(string text)
      {
         return new Token(TokenType.Value, text, TokenModifier.None);
      }

      public static Token Assignment(string text)
      {
         return new Token(TokenType.Assignment, text, TokenModifier.None);
      }

      private Token(TokenType type, string text, TokenModifier modifier)
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
         if (obj is Token)
            return Equals((Token)obj);

         return false;
      }

      public static bool operator ==(Token a, Token b)
      {
         return a.Equals(b);
      }

      public static bool operator !=(Token a, Token b)
      {
         return !a.Equals(b);
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
