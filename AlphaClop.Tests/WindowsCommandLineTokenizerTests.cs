#define CONTRACTS_FULL
using Alphaleonis.CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace AlphaClop.Tests
{
   //public class WindowsCommandLineTokenizerTests
   //{
   //   private IReadOnlyList<Token> GetTokens(string commandLine)
   //   {
   //      WindowsCommandLineTokenizer tokenizer = new WindowsCommandLineTokenizer();
   //      return tokenizer.Tokenize(commandLine);
   //   }


   //   public static IEnumerable<object[]> TokenizerTestData
   //   {
   //      get
   //      {
   //         // Test values only, including escape characters and quotation marks.
   //         yield return new object[] { @"MyApp alpha beta", new[] { Token.Value("MyApp"), Token.Value("alpha"), Token.Value("beta") } };
   //         yield return new object[] { @"MyApp ""alpha with spaces"" ""beta with spaces""", new[] { Token.Value("MyApp"), Token.QuotedValue("alpha with spaces"), Token.QuotedValue("beta with spaces") } };
   //         yield return new object[] { @"MyApp 'alpha with spaces' beta", new[] { Token.Value("MyApp"), Token.Value("'alpha"), Token.Value("with"), Token.Value("spaces'"), Token.Value("beta") } };
   //         yield return new object[] { @"MyApp \\\alpha \\\\""beta", new[] { Token.Value("MyApp"), Token.Value(@"\\\alpha"), Token.Value(@"\\beta") } };
   //         yield return new object[] { @"MyApp \\\\\""alpha \""beta", new[] { Token.Value("MyApp"), Token.Value(@"\\""alpha"), Token.Value(@"""beta") } };
   //         yield return new object[] { @"MyApp ""alpha\with\something\\\""quote\\\"""" beta", new[] { Token.Value("MyApp"), Token.QuotedValue(@"alpha\with\something\""quote\"""), Token.Value(@"beta") } };
   //      }
   //   }

   //   [Theory]            
   //   [MemberData(nameof(TokenizerTestData))]
   //   private void TestMethod(string commandLine, IEnumerable<Token> expectedTokens)
   //   {
   //      var actualTokens = GetTokens(commandLine);
   //      Assert.Equal(expectedTokens, actualTokens);
         
   //   }

   //   [Fact]
   //   private void TestMethod()
   //   {
   //      string s = Environment.GetEnvironmentVariable("ii") ?? null;
         
   //      Test(s);

   //   }

   //   private void Test(string s)
   //   {
   //      Contract.Requires(s != null);
   //   }
   //}
}
