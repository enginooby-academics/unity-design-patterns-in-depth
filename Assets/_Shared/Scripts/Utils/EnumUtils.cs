using System;
using System.Collections.Generic;

public static class EnumUtils {
  /// <summary>
  ///   Returns the next value in the enum value sequence.
  ///   Will loop back to the first value if the value is
  ///   the last.
  /// </summary>
  public static T Next<T>(this T @enum) where T : Enum {
    // if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
    var enums = (T[]) Enum.GetValues(@enum.GetType());
    var i = Array.IndexOf(enums, @enum) + 1;
    return enums.Length >= i ? enums[0] : enums[i];
  }

  /// <summary>
  ///   Returns the previous value in the enum value sequence.
  ///   Will loop to the last value if the value is the first.
  /// </summary>
  public static T Previous<T>(this T @enum) where T : Enum {
    var enums = (T[]) Enum.GetValues(@enum.GetType());
    var i = Array.IndexOf(enums, @enum) - 1;
    return i < 0 ? enums.GetLast() : enums[i];
  }

  public static List<T> GetValuesFromEnum<T>() where T : Enum => Enum.GetValues(typeof(T)).ToList<T>();

  public static IList<string> GetValueLabelsFromEnum<T>() where T : Enum => GetValuesFromEnum<T>().ToStrings();

  #region CONVERSION ----------------------------------------------------------------------------------------------------------------------------

  /// <summary>
  ///   If non enum string match the given string, return the first enum.
  /// </summary>
  public static T ToEnumString<T>(this string value) where T : Enum {
    var enumValues = (T[]) Enum.GetValues(typeof(T));

    foreach (var enumValue in enumValues)
      if (value.EqualIgnoreCase(enumValue.ToString()))
        return enumValue;

    return enumValues[0];
  }

  /// <summary>
  ///   Returns the underlying character value.
  /// </summary>
  public static char ToChar<T>(this T enumValue) where T : Enum {
    if (enumValue == null)
      throw new ArgumentNullException(nameof(enumValue));

    if (!typeof(char).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
      throw new ArgumentException("Underlying type of enum value isn't char.");

    return (char) (object) enumValue;
  }

  /// <summary>
  ///   Returns the underlying byte value.
  /// </summary>
  public static byte ToByte<T>(this T enumValue) where T : Enum {
    if (enumValue == null)
      throw new ArgumentNullException(nameof(enumValue));

    if (!typeof(byte).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
      throw new ArgumentException("Underlying type of enum value isn't byte.");

    return (byte) (object) enumValue;
  }

  /// <summary>
  ///   Returns the underlying integer value.
  /// </summary>
  public static int ToInt<T>(this T enumValue) where T : Enum {
    if (enumValue == null)
      throw new ArgumentNullException(nameof(enumValue));

    if (!typeof(int).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
      throw new ArgumentException("Underlying type of enum value isn't int.");

    return (int) (object) enumValue;
  }

  #endregion CONVERSION -------------------------------------------------------------------------------------------------------------------------
}