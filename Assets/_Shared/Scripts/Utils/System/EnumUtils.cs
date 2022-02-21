using System;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Utils;

public static class EnumUtils {
  public static bool HasFlags(this Enum @enum, params Enum[] flags) => flags.All(@enum.HasFlag);

  #region RETRIEVAL

  // ===================================================================================================================

  /// <summary>
  ///   Returns the next value in the enum value sequence.
  ///   Will loop back to the first value if the value is the last.
  /// </summary>
  public static T Next<T>(this T @enum) where T : Enum {
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

  public static IEnumerable<T> GetValuesFromEnum<T>() where T : Enum => Enum.GetValues(typeof(T)).ToList<T>();

  public static IEnumerable<string> GetValueLabelsFromEnum<T>() where T : Enum => GetValuesFromEnum<T>().ToStrings();

  #endregion

  #region CONVERSION

  // ===================================================================================================================

  /// <summary>
  ///   If non enum string match the given string, return the first enum.
  /// </summary>
  public static T ToEnumString<T>(this string value) where T : Enum {
    var enumValues = (T[]) Enum.GetValues(typeof(T));

    foreach (var enumValue in enumValues)
      if (value.EqualsIgnoreCase(enumValue.ToString()))
        return enumValue;

    return enumValues[0];
  }

  /// <summary>
  ///   Returns the underlying character value.
  /// </summary>
  public static char ToChar<T>(this T enumValue) where T : Enum => enumValue.To<T, char>();

  /// <summary>
  ///   Returns the underlying byte value.
  /// </summary>
  public static byte ToByte<T>(this T enumValue) where T : Enum => enumValue.To<T, byte>();

  /// <summary>
  ///   Returns the underlying integer value.
  /// </summary>
  public static int ToInt<T>(this T enumValue) where T : Enum => enumValue.To<T, int>();

  /// <summary>
  ///   Returns the value of underlying target type.
  /// </summary>
  public static TTarget To<TEnum, TTarget>(this TEnum enumValue) where TEnum : Enum {
    if (enumValue == null)
      throw new ArgumentNullException(nameof(enumValue));

    if (!typeof(TTarget).IsAssignableFrom(Enum.GetUnderlyingType(typeof(TEnum))))
      throw new ArgumentException("Underlying type of enum value isn't " + typeof(TTarget).Name);

    return (TTarget) (object) enumValue;
  }

  #endregion
}