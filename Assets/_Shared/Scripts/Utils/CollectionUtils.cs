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
  [Obsolete("Use " + nameof(DestroyWithGameObject))]
  public static void Destroy<T>(this IEnumerable<T> components) where T : MonoBehaviour {
    DestroyWithGameObject(components);
  }

  public static void DestroyWithGameObject<T>(this IEnumerable<T> components) where T : MonoBehaviour {
    foreach (var component in components) {
#if UNITY_EDITOR
      if (!EditorApplication.isPlaying) Object.DestroyImmediate(component.gameObject);
#endif
      Object.Destroy(component.gameObject);
    }
  }

  /// <summary>
  /// Return true if not contains.
  /// </summary>
  public static bool AddIfNotContains<T>(this IList<T> list, T element) {
    if (list.Contains(element)) return false;

    list.Add(element);
    return true;
  }

  public static List<T> ToList<T>(this Array array) => array.Cast<T>().ToList();

  /// <summary>
  /// Perform an action on each item.
  /// </summary>
  public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
    foreach (var obj in collection)
      action(obj);
    return collection;
  }

  #region VALIDATION

  public static bool HasIndex<T>(this IEnumerable<T> collection, int index) =>
    index.IsInRange(0, collection.Count() - 1);

  // TIP: Use Obsolete attribute to safely rename methods used in multiple projects
  // Delete obsoleted method when all consumer projects update the renamed method
  [Obsolete("Use HasIndex")]
  public static bool ValidateIndex<T>(this IEnumerable<T> list, int index) => 0 <= index && index < list.Count();

  // TIP: Use interface (IList) instead of concrete class (List) to cover more types in extension method
  /// <summary>
  ///   Check if collection is null or empty.
  /// </summary>
  public static bool IsUnset<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

  public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

  /// <summary>
  ///   Check if collection is not null nor empty.
  /// </summary>
  public static bool IsSet<T>(this IEnumerable<T> list) => list != null && list.Any();

  #endregion

  #region ELEMENT RETRIEVAL

  [Obsolete("Use GetNext")]
  public static T NavNext<T>(this IList<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var nextIndex = currentIndex == list.Count - 1 ? 0 : currentIndex + 1;
    return list[nextIndex];
  }

  /// <summary>
  ///   Add method IndexOf() to IReadOnlyList.
  /// </summary>
  public static int IndexOf<T>(this IEnumerable<T> self, T elementToFind) {
    var i = 0;
    foreach (var element in self) {
      if (Equals(element, elementToFind))
        return i;
      i++;
    }

    return -1;
  }

  public static T GetNext<T>(this IEnumerable<T> collection, T currentItem) {
    var currentIndex = collection.IndexOf(currentItem);
    var nextIndex = currentIndex == collection.Count() - 1 ? 0 : currentIndex + 1;
    return collection.ElementAt(nextIndex);
  }

  [Obsolete("Use GetPrevious")]
  public static T NavPrevious<T>(this IList<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }

  public static T GetPrevious<T>(this IEnumerable<T> list, T currentItem) {
    var currentIndex = list.IndexOf(currentItem);
    var previousIndex = currentIndex == 0 ? list.Count() - 1 : currentIndex - 1;
    return list.ElementAt(previousIndex);
  }

  public static T GetLast<T>(this IEnumerable<T> collection) => collection.ElementAt(collection.Count() - 1);

  public static void RemoveLast<T>(this IList<T> list) {
    if (!list.IsSet()) return;

    list.RemoveAt(list.Count - 1);
  }

  public static T GetRandom<T>(this IEnumerable<T> list) => list.ElementAt(Random.Range(0, list.Count()));

  /// <summary>
  ///   Return a random element which is different than the given one (can be null).
  /// </summary>
  public static T GetRandomOtherThan<T>(this IEnumerable<T> collection, T excludingElement) {
    if (excludingElement == null || collection.Count() == 1) return collection.GetRandom();

    var excludingIndex = collection.IndexOf(excludingElement);
    var randomIndex = excludingIndex;
    while (randomIndex == excludingIndex) randomIndex = Random.Range(0, collection.Count());
    return collection.ElementAt(randomIndex);
  }

  /// <summary>
  ///   Return true if the element exists in the array and not null.
  /// </summary>
  public static bool TryGetById<T>(this IEnumerable<T> collection, int id, out T element) {
    if (collection.HasIndex(id)) {
      element = collection.ElementAt(id);
      return true;
    }

    element = default(T);
    return false;
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
      (list[k], list[n]) = (list[n], list[k]);
    }
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

  // https://stackoverflow.com/questions/1211608/possible-to-iterate-backwards-through-a-foreach
  public static IEnumerable<T> FastReverse<T>(this IList<T> list) {
    for (var i = list.Count - 1; i >= 0; i--) yield return list[i];
  }

  #endregion
}