using System;

public static class UnclassifiedUtils {
  public static bool EqualIgnoreCase(this string string1, string string2) {
    return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
  }
}

