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
}