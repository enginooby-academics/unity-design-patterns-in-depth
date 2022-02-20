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

  public static string ToSentenceCase(this string str) {
    return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
  }

  public static bool IsEmpty(this string @string) => @string == "";

  public static bool EqualsIgnoreCase(this string string1, string string2) =>
    string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
}