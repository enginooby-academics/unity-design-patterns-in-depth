using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnityObjectUtils {
  // ! not pass by ref => not modify original list
  public static IEnumerable<T> OrderByName<T>(this IEnumerable<T> list) where T : Object {
    return list.OrderBy(item => item.name);
  }
}