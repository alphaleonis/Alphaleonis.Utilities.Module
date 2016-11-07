using System;
using System.Runtime.Serialization;

namespace Alphaleonis.CommandLine
{
   [Serializable]
   public class InvalidCommandLineParserConfigurationException : Exception
   {
      public InvalidCommandLineParserConfigurationException() { }
      public InvalidCommandLineParserConfigurationException(string message) : base(message) { }
      public InvalidCommandLineParserConfigurationException(string message, Exception innerException) : base(message, innerException) { }
      protected InvalidCommandLineParserConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
   }
}