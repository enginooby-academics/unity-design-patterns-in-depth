using System;
using System.Collections.Generic;
using UnityEngine;

// TIP: To rename an API in the shared library
// 1. Rename in current project
// 2. Duplicate API and manually rename one to old name to avoid error in other projects
// 3. Use Obsolete attribute on the old API to safely rename methods used in multiple projects
// 3. Delete obsoleted method when all consumer projects update the renamed method
public static class _ObsoleteUtils {
  [Obsolete("Use " + nameof(CollectionUtils.HasIndex) + "")]
  public static bool ValidateIndex<T>(this IEnumerable<T> list, int index) => list.HasIndex(index);

  [Obsolete("Use " + nameof(NumeralUtils.FromRadianToDegree) + "")]
  public static decimal ToDegree(this float radian, int decimals = 0) => radian.FromRadianToDegree();

  [Obsolete("Use " + nameof(CollectionUtils.DestroyWithGameObject) + "")]
  public static void Destroy<T>(this IEnumerable<T> components) where T : MonoBehaviour =>
    components.DestroyWithGameObject();

  [Obsolete("Use " + nameof(CollectionUtils.GetNext))]
  public static T NavNext<T>(this IEnumerable<T> list, T currentItem) => list.GetNext(currentItem);

  [Obsolete("Use " + nameof(CollectionUtils.GetPrevious))]
  public static T NavPrevious<T>(this IEnumerable<T> list, T currentItem) => list.GetPrevious(currentItem);

  [Obsolete("Use " + nameof(NumeralUtils.ToWord))]
  public static string NumberToWord(int number) => number.ToWord();

  [Obsolete("Use " + nameof(StringUtils.EqualsIgnoreCase))]
  public static bool EqualIgnoreCase(this string string1, string string2) => string1.EqualsIgnoreCase(string2);

  [Obsolete("Use " + nameof(TransformUtils.SetPosX))]
  public static void PosX(this Transform transform, float x) => transform.SetPosX(x);

  [Obsolete("Use " + nameof(TransformUtils.SetPosY))]
  public static void PosY(this Transform transform, float y) => transform.SetPosY(y);

  [Obsolete("Use " + nameof(TransformUtils.SetPosZ))]
  public static void PosZ(this Transform transform, float z) => transform.SetPosZ(z);
}