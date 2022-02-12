using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class CollectionUtils {
  /// <summary>
  ///   Destroy all GameObjects of the component list.
  /// </summary>
  public static void Destroy<T>(this T[] components) where T : MonoBehaviour {
    foreach (var component in components) {
#if UNITY_EDITOR
      if (!EditorApplication.isPlaying) Object.DestroyImmediate(component.gameObject);
#endif
      Object.Destroy(component.gameObject);
    }
  }

  public static List<T> ToList<T>(this Array array) {
    var list = new List<T>();

    foreach (var item in array) list.Add((T) item);

    return list;
  }

  #region VALIDATION

  public static bool HasIndex<T>(this T[] collection, int index) => index.IsInRange(0, collection.Length - 1);

  public static bool HasIndex<T>(this IList<T> collection, int index) => index.IsInRange(0, collection.Count - 1);

  // TIP: Use Obsolete attribute to safely rename methods used in multiple projects
  // Delete obsoleted method when all consumer projects update the renamed method
  [Obsolete("Use HasIndex")]
  public static bool ValidateIndex<T>(this IList<T> list, int index) => 0 <= index && index < list.Count;

  // TIP: Use interface (IList) instead of concrete class (List) to cover more types in extension method
  /// <summary>
  ///   Check if collection is null or empty.
  /// </summary>
  public static bool IsUnset<T>(this IList<T> list) => list == null || list.Count == 0;

  public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

  /// <summary>
  ///   Check if collection is not null nor empty.
  /// </summary>
  public static bool IsSet<T>(this IList<T> list) => list != null && list.Count > 0;

  #endregion

  #region ELEMENT RETRIEVAL

  // TODO: rename GetNext
  public static T NavNext<T>(this IList<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var nextIndex = currentIndex == list.Count - 1 ? 0 : currentIndex + 1;
    return list[nextIndex];
  }

  /// <summary>
  ///   Add method IndexOf() to IReadOnlyList.
  /// </summary>
  public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind) {
    var i = 0;
    foreach (var element in self) {
      if (Equals(element, elementToFind))
        return i;
      i++;
    }

    return -1;
  }

  public static T GetNext<T>(this IReadOnlyList<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var nextIndex = currentIndex == list.Count - 1 ? 0 : currentIndex + 1;
    return list[nextIndex];
  }

  public static T NavPrevious<T>(this IList<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }

  public static T GetPrevious<T>(this IReadOnlyList<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }

  public static T GetLast<T>(this IList<T> list) => list[list.Count - 1];

  public static void RemoveLast<T>(this IList<T> list) {
    if (!list.IsSet()) return;

    list.RemoveAt(list.Count - 1);
  }

  public static T GetRandom<T>(this IList<T> list) => list[Random.Range(0, list.Count)];

  /// <summary>
  ///   Return a random element which is different than the given one (can be null).
  /// </summary>
  public static T GetRandomOtherThan<T>(this IList<T> list, T excludingElement) {
    if (excludingElement == null || list.Count == 1) return list.GetRandom();
    var excludingIndex = list.IndexOf(excludingElement);
    var randomIndex = excludingIndex;
    while (randomIndex == excludingIndex) randomIndex = Random.Range(0, list.Count);
    return list[randomIndex];
  }

  /// <summary>
  ///   Return true if the element exists and not null.
  /// </summary>
  public static bool TryGetById<T>(this IList<Object> objects, int id, out T element) where T : Object {
    if (objects.Count > id) {
      element = objects[id] as T;
      return element ? true : false;
    }

    element = null;
    return false;
  }

  /// <summary>
  ///   Return true if the element exists in the array and not null.
  /// </summary>
  public static bool TryGetById<T>(this Object[] objects, int id, out T element) where T : Object {
    if (objects.Length > id) {
      element = objects[id] as T;
      return element ? true : false;
    }

    element = null;
    return false;
  }

  /// <summary>
  ///   Return true if the element exists in the array and not null.
  /// </summary>
  public static bool TryGetById(this Object[] objects, int id, out Object element) {
    if (objects.Length > id) {
      element = objects[id];
      return element ? true : false;
    }

    element = null;
    return false;
  }

  // ? Move to GameObjectUtils
  /// <summary>
  ///   Return the GameObject in the list which is nearest to the given position.
  /// </summary>
  // REFACTOR
  public static GameObject GetNearestTo(this IList<GameObject> list, Vector3 pos) {
    var nearestIndex = 0;
    var lastDist = Mathf.Infinity;

    for (var i = 0; i < list.Count; i++) {
      var gameObject = list[i];
      var distanceToPos = Vector3.Distance(pos, gameObject.transform.position);
      if (distanceToPos < lastDist) {
        nearestIndex = i;
        lastDist = distanceToPos;
      }
    }

    return list[nearestIndex];
  }

  /// <summary>
  ///   Return the GameObject in the list which is nearest to the given position.
  /// </summary>
  public static GameObject GetNearestTo(this IReadOnlyList<GameObject> list, Vector3 pos) {
    var nearestIndex = 0;
    var lastDist = Mathf.Infinity;

    for (var i = 0; i < list.Count; i++) {
      var gameObject = list[i];
      var distanceToPos = Vector3.Distance(pos, gameObject.transform.position);
      if (distanceToPos < lastDist) {
        nearestIndex = i;
        lastDist = distanceToPos;
      }
    }

    return list[nearestIndex];
  }

  #endregion

  #region COLLECTION OPERATIONS

  private static readonly System.Random rng = new System.Random();

  public static void Shuffle<T>(this IList<T> list) {
    Debug.Log("Shuffle");
    var n = list.Count;
    while (n > 1) {
      n--;
      var k = rng.Next(n + 1);
      var value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
  }

  // ! not pass by ref => not modify original list
  public static List<T> OrderByName<T>(this IList<T> list) where T : Object {
    return list.OrderBy(item => item.name).ToList();
  }

  public static void RemoveNullEntries<T>(this IList<T> list) where T : class {
    for (var i = list.Count - 1; i >= 0; i--)
      if (Equals(list[i], null))
        list.RemoveAt(i);
  }

  public static void RemoveDefaultValues<T>(this IList<T> list) {
    for (var i = list.Count - 1; i >= 0; i--)
      if (Equals(default(T), list[i]))
        list.RemoveAt(i);
  }

  #endregion
}