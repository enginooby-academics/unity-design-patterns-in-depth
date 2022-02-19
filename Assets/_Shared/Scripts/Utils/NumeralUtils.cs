using System;
using UnityEngine;

public static class NumeralUtils {
  /// <summary>
  ///   Returns whether the value is greater than or equal to a minimal value
  ///   and smaller than or equal to a maximum value.
  /// </summary>
  public static bool IsInRange(this int number, int min, int max) => number >= min && number <= max;

  public static bool IsEven(this int number) => number % 2 == 0;

  public static bool IsOdd(this int number) => number % 2 != 0;

  /// <summary>
  ///   Return false if 0.
  /// </summary>
  public static bool ToBool(this int number) => number != 0;

  /// <summary>
  ///   Return a Vector3 whose x, y, z equal given value. E.g., 1 -> (1, 1, 1).
  /// </summary>
  public static Vector3 ToVector3(this float number) => new Vector3(number, number, number);

  /// <summary>
  ///   Round the calculated degree to a given number of decimal places.
  /// </summary>
  public static decimal FromRadianToDegree(this float radian, int decimals = 0) =>
    decimal.Round((decimal) (radian * Mathf.Rad2Deg), decimals);

  public static float FromDegreeToRadian(this float degree) => (float) Math.PI / 180f * degree;

  [Obsolete("Use FromRadianToDegree")]
  public static decimal ToDegree(this float radian, int decimals = 0) => radian.FromRadianToDegree();

  /// <summary>
  ///   Convert number to English word: one, two, etc.
  /// </summary>
  public static string ToWord(this int number) {
    if (number == 0) return "zero";

    if (number < 0) return "minus " + ToWord(Math.Abs(number));

    var words = "";

    if (number / 1000000 > 0) {
      words += ToWord(number / 1000000) + " million ";
      number %= 1000000;
    }

    if (number / 1000 > 0) {
      words += ToWord(number / 1000) + " thousand ";
      number %= 1000;
    }

    if (number / 100 > 0) {
      words += ToWord(number / 100) + " hundred ";
      number %= 100;
    }

    if (number > 0) {
      if (words != "")
        words += "and ";

      var unitsMap = new[] {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
        "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
      };
      var tensMap = new[] {"zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"};

      if (number < 20) {
        words += unitsMap[number];
      }
      else {
        words += tensMap[number / 10];
        if (number % 10 > 0)
          words += "-" + unitsMap[number % 10];
      }
    }

    return words;
  }

  [Obsolete("Use ToWord")]
  public static string NumberToWord(int number) => number.ToWord();

  /// <summary>
  ///   Returns the normalized (between 0 and 1) value.
  /// </summary>
  public static float Normalize(this float number, float min, float max) => (number - min) / (max - min);

  /// <summary>
  ///   Returns the value mapped to a new scale.
  /// </summary>
  public static float Map(this float number, float min, float max, float targetMin, float targetMax) =>
    (number - min) * ((targetMax - targetMin) / (max - min)) + targetMin;

  public static int WithRandomSign(this int number) {
    var sign = UnityEngine.Random.Range(0, 100) > 50 ? 1 : -1;
    return sign * number;
  }
}