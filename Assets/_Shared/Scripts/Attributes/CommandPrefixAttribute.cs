using System;

namespace Enginooby.Attribute {
// #if ASSET_QUANTUM_CONSOLE
//   /// <inheritdoc />
//   public class CommandAttribute : QFSW.QC.CommandAttribute { }
// #else
  // Fallback attribute

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public sealed class CommandPrefixAttribute : System.Attribute {
    public readonly string value;

    public CommandPrefixAttribute(string value) => value = value;
  }
}