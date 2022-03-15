using System;

namespace Enginooby.Attribute {
// #if ASSET_QUANTUM_CONSOLE
//   /// <inheritdoc />
//   public class CommandAttribute : QFSW.QC.CommandAttribute { }
// #else
  // Fallback attribute

  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true,
    Inherited = false)]
  public sealed class CommandAttribute : System.Attribute {
    public readonly string Alias;

    public CommandAttribute(string @alias) => Alias = alias;
  }
}