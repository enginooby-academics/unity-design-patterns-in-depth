using System;
using System.Collections.Generic;
using System.Linq;

public static class StringUtils {
  public static string RemoveWhitespace(this string input) {
    return new string(input.ToCharArray()
        .Where(c => !Char.IsWhiteSpace(c))
        .ToArray());
  }

  public static List<string> ToStrings<T>(this IList<T> list) where T : IFormattable {
    var stringList = new List<string>();

    foreach (var item in list) {
      stringList.Add(item.ToString());
    }

    return stringList;
  }

  public static string ToSentenceCase(this string str) {
    return System.Text.RegularExpressions.Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
  }

  public static bool EqualIgnoreCase(this string string1, string string2) {
    return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
  }
}

