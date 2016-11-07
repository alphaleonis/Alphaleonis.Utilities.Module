using Alphaleonis.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
   class Program
   {
      static void Main(string[] args)
      {
         //args = new string[] 
         //{
         //   "-a  : hej",
         //   "pelle",
         //   "-b",
         //   "=",
         //   "-kk",
         //   "some value",
         //   "-c",
         //   "--o",
         //   "-p½a",
         //   "other value",
         //   "-d",
         //   "=value"
         //};

         WindowsStyleLexer lexer = new WindowsStyleLexer();
         var tokens = lexer.Tokenize(args);
         foreach (var token in tokens)
         {
            Console.WriteLine($"{token.TokenType} = \"{token.Text}\"");
         }
      }
   }
}
