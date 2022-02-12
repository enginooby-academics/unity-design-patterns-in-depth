using System;

/// <summary>
///   This attribute is used to represent a string value for a value in an enum.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute : Attribute {
  public StringValueAttribute(string value) => StringValue = value;

  /// <summary>
  ///   Holds the string value for a value in an enum.
  /// </summary>
  public string StringValue { get; protected set; }
}

public static class StringValueUtils {
  /// <summary>
  ///   [For EnumString]
  ///   Will get the string value for a given enums value, this will
  ///   only work if you assign the StringValue attribute to
  ///   the items in your enum.
  /// </summary>
  public static string ToString(this Enum value) {
    var type = value.GetType();
    var fieldInfo = type.GetField(value.ToString());
    var stringValueAttrs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

    // Return the first if there was a match.
    return stringValueAttrs!.Length > 0 ? stringValueAttrs[0].StringValue : null;
  }
}