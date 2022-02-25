using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringUtils {
  public static string RemoveWhitespace(this string input) {
    return new string(input.ToCharArray()
      .Where(c => !char.IsWhiteSpace(c))
      .ToArray());
  }

  public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> list) where T : IFormattable {
    return list.Select(item => item.ToString());
  }

  /// <summary>
  ///   Add space before and lower every capital character except the 1st one to form a normal sentence.
  ///   <example>TheAppDemo -> The app demo</example>
  /// </summary>
  public static string ToSentenceLowerCase(this string str) {
    return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
  }

  /// <summary>
  ///   Add a space before every capital character.
  ///   <example>TheAppDemo -> The app demo</example>
  /// </summary>
  public static string AddSpacesBeforeCapitals(this string @string) {
    return Regex.Replace(@string, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
  }

  [Obsolete("Use " + nameof(ToSentenceLowerCase))]
  public static string ToSentenceCase(this string str) => str.ToSentenceLowerCase();

  public static bool IsEmpty(this string @string) => @string.Equals(string.Empty);

  public static bool EqualsIgnoreCase(this string string1, string string2) =>
    string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
}