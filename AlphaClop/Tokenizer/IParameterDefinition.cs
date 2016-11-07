using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Alphaleonis.CommandLine
{
   public interface IParserContext
   {
      ICommandLineDefinition CommandLineDefinition { get; }
   }

   public interface IArgumentValidator
   {
      void Validate(object values);
   }

   public interface IParameterDefinition
   {
      IReadOnlyList<string> Aliases { get; }
      bool IsMandatory { get; }
      string Name { get; }
      Type ParameterType { get; }
      int? Position { get; }
      IReadOnlyList<IArgumentValidator> Validators { get; }
   }

   public interface IParameterSetDefinition
   {
      string Name { get; }
      IReadOnlyList<IParameterDefinition> Parameters { get; }
   }

   public interface IVerbDefinition
   {
      string Verb { get; }
      IReadOnlyList<IParameterSetDefinition> ParameterSets { get; }
   }

   public interface ICommandLineDefinition
   {
      IReadOnlyList<IVerbDefinition> Verbs { get; }
   }

   internal interface ILexer
   {
      void ValidateParameterNames(IParameterDefinition parameter);
      string GetOptionDisplayText(IParameterDefinition parameter);
      IReadOnlyList<Token> Tokenize(string[] args);
   }

   internal interface IArgument
   {
      string Name { get; }
      IParameterDefinition Parameter { get; }      
      object Value { get; }
   }

   internal interface IArgumentSet
   {
      IParameterSetDefinition ParameterSet { get; }
      IReadOnlyList<IArgument> Arguments { get; }
   }

   internal interface IParser
   {
      IReadOnlyList<IArgumentSet> Parse(string[] args);
   }
}