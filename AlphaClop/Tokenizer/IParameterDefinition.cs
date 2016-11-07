using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Alphaleonis.CommandLine
{
   public class ParserOptions
   {
      public bool IgnoreCase { get; set; }
   }

   public class ParameterDefinition 
   {
      public ParameterDefinition(ParserOptions options, IEnumerable<string> names, bool isMandatory, Type parameterType, int? position, IReadOnlyList<IArgumentValidator> validators)
      {
         if (names == null)
            throw new ArgumentNullException(nameof(names), $"{nameof(names)} is null.");

         if (!names.Any())
            throw new ArgumentException($"{nameof(names)} is empty.", nameof(names));

         if (options == null)
            throw new ArgumentNullException(nameof(options), $"{nameof(options)} is null.");

         if (parameterType == null)
            throw new ArgumentNullException(nameof(parameterType), $"{nameof(parameterType)} is null.");

         if (validators == null || validators.Count == 0)
            throw new ArgumentException($"{nameof(validators)} is null or empty.", nameof(validators));

         m_names = names;
         Options = options;
         IsMandatory = isMandatory;
         Names = names;
         ParameterType = parameterType;
         Position = position;
         Validators = validators;
      }

      public ParserOptions Options { get; }
      public bool IsMandatory { get; }
      public IReadOnlyList<string> Names { get; }
      public Type ParameterType { get; }
      public int? Position { get; }
      public IReadOnlyList<IArgumentValidator> Validators { get; }
   }

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