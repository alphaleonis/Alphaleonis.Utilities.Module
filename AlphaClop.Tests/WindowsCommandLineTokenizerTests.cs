using Alphaleonis.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace AlphaClop.Tests
{
   public class WindowsCommandLineTokenizerTests
   {
      private IReadOnlyList<Token> GetTokens(string commandLine)
      {
         WindowsCommandLineTokenizer tokenizer = new WindowsCommandLineTokenizer();
         return tokenizer.Tokenize(commandLine);
      }


      public static IEnumerable<object[]> TokenizerTestData
      {
         get
         {
            // Test values only, including escape characters and quotation marks.
            yield return new object[] { @"MyApp alpha beta", new[] { Token.UnqotedValue("MyApp"), Token.UnqotedValue("alpha"), Token.UnqotedValue("beta") } };
            yield return new object[] { @"MyApp ""alpha with spaces"" ""beta with spaces""", new[] { Token.UnqotedValue("MyApp"), Token.QuotedValue("alpha with spaces"), Token.QuotedValue("beta with spaces") } };
            yield return new object[] { @"MyApp 'alpha with spaces' beta", new[] { Token.UnqotedValue("MyApp"), Token.UnqotedValue("'alpha"), Token.UnqotedValue("with"), Token.UnqotedValue("spaces'"), Token.UnqotedValue("beta") } };
            yield return new object[] { @"MyApp \\\alpha \\\\""beta", new[] { Token.UnqotedValue("MyApp"), Token.UnqotedValue(@"\\\alpha"), Token.UnqotedValue(@"\\beta") } };
            yield return new object[] { @"MyApp \\\\\""alpha \""beta", new[] { Token.UnqotedValue("MyApp"), Token.UnqotedValue(@"\\""alpha"), Token.UnqotedValue(@"""beta") } };
            yield return new object[] { @"MyApp ""alpha\with\something\\\""quote\\\"""" beta", new[] { Token.UnqotedValue("MyApp"), Token.QuotedValue(@"alpha\with\something\""quote\"""), Token.UnqotedValue(@"beta") } };
         }
      }

      [Theory]            
      [MemberData(nameof(TokenizerTestData))]
      private void TestMethod(string commandLine, IEnumerable<Token> expectedTokens)
      {
         var actualTokens = GetTokens(commandLine);
         Assert.Equal(expectedTokens, actualTokens);
         
      }
   }
}
