using System;

/// <summary>
/// This attribute is used to represent a string value for a value in an enum.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute : Attribute {
  /// <summary>
  /// Holds the string value for a value in an enum.
  /// </summary>
  public string StringValue { get; protected set; }

  /// <summary>
  /// Constructor used to init a StringValue Attribute.
  /// </summary>
  /// <param name="value"></param>
  public StringValueAttribute(string value) {
    this.StringValue = value;
  }
}