using System;
using System.Reflection;

public static class EnumUtils {
  /// <summary>
  /// [For EnumString]
  /// Will get the string value for a given enums value, this will
  /// only work if you assign the StringValue attribute to
  /// the items in your enum.
  /// </summary>
  public static string ToString(this Enum value) {
    Type type = value.GetType();
    FieldInfo fieldInfo = type.GetField(value.ToString());
    StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
        typeof(StringValueAttribute), false) as StringValueAttribute[];

    // Return the first if there was a match.
    return attribs.Length > 0 ? attribs[0].StringValue : null;
  }

  /// <summary>
  /// If non enum string match the given string, return the first enum.
  /// </summary>
  public static T ToEnumString<T>(this string value) where T : Enum {
    var enumValues = (T[])Enum.GetValues(typeof(T));
    foreach (var enumValue in enumValues) {
      if (value.EqualIgnoreCase(enumValue.ToString())) return enumValue;
    }

    return enumValues[0];
  }

  /// <summary>
  /// Increment (loop) enum value.
  /// </summary>
  public static T Next<T>(this T src) where T : struct {
    if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

    T[] Arr = (T[])Enum.GetValues(src.GetType());
    int j = Array.IndexOf<T>(Arr, src) + 1;
    return (Arr.Length == j) ? Arr[0] : Arr[j];
  }

  /// <summary>
  /// Decrement (loop) enum value.
  /// </summary>
  public static T Previous<T>(this T src) where T : struct {
    if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

    T[] Arr = (T[])Enum.GetValues(src.GetType());
    int j = Array.IndexOf<T>(Arr, src) - 1;
    return (j <= -1) ? Arr[Arr.Length - 1] : Arr[j];
  }
}

