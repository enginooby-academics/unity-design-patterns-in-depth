using System;
using UnityEngine;

public static class NumeralUtils {
  /// <summary>
  /// Round the calculated degree to a given number of decimal places.
  /// </summary>
  public static decimal ToDegree(this float radian, int decimals = 0) {
    return Decimal.Round((decimal)(radian * Mathf.Rad2Deg), decimals);
  }

  public static bool IsEven(this int value) {
    return value % 2 == 0;
  }

  public static bool IsOdd(this int value) {
    return value % 2 != 0;
  }

  /// <summary>
  /// Convert number to English word: one, two, etc.
  /// </summary>
  public static string ToWord(this int number) {
    return NumberToWord(number);
  }

  public static string NumberToWord(int number) {
    if (number == 0) return "zero";

    if (number < 0) return "minus " + NumberToWord(Math.Abs(number));

    string words = "";

    if ((number / 1000000) > 0) {
      words += NumberToWord(number / 1000000) + " million ";
      number %= 1000000;
    }

    if ((number / 1000) > 0) {
      words += NumberToWord(number / 1000) + " thousand ";
      number %= 1000;
    }

    if ((number / 100) > 0) {
      words += NumberToWord(number / 100) + " hundred ";
      number %= 100;
    }

    if (number > 0) {
      if (words != "")
        words += "and ";

      var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
      var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

      if (number < 20)
        words += unitsMap[number];
      else {
        words += tensMap[number / 10];
        if ((number % 10) > 0)
          words += "-" + unitsMap[number % 10];
      }
    }

    return words;
  }
}