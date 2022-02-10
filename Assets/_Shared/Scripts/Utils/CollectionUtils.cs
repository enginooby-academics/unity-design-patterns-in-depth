using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public static class CollectionUtils {
  #region VALIDATION
  public static bool HasIndex<T>(this T[] collection, int index) => index.IsInRange(0, collection.Length - 1);
  public static bool HasIndex<T>(this IList<T> collection, int index) => index.IsInRange(0, collection.Count - 1);

  // TIP: Use Obsolete attribute to safely rename methods used in multiple projects
  // Delete obsoleted method when all consumer projects update the renamed method
  [Obsolete("Use HasIndex")]
  public static bool ValidateIndex<T>(this IList<T> list, int index) {
    return 0 <= index && index < list.Count;
  }

  // TIP: Use interface (IList) instead of concrete class (List) to cover more types in extension method
  /// <summary>
  /// Check if collection is null or empty.
  /// </summary>
  public static bool IsUnset<T>(this IList<T> list) {
    return list == null || list.Count == 0;
  }

  public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

  /// <summary>
  /// Check if collection is not null nor empty.
  /// </summary>
  public static bool IsSet<T>(this IList<T> list) {
    return list != null && list.Count > 0;
  }
  #endregion

  #region ELEMENT RETRIEVAL
  // TODO: rename GetNext
  public static T NavNext<T>(this IList<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int nextIndex = currentIndex == list.Count - 1 ? 0 : currentIndex + 1;
    return list[nextIndex];
  }

  /// <summary>
  /// Add method IndexOf() to IReadOnlyList.
  /// </summary>
  public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind) {
    int i = 0;
    foreach (T element in self) {
      if (Equals(element, elementToFind))
        return i;
      i++;
    }
    return -1;
  }

  public static T GetNext<T>(this IReadOnlyList<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int nextIndex = currentIndex == list.Count - 1 ? 0 : currentIndex + 1;
    return list[nextIndex];
  }

  public static T NavPrevious<T>(this IList<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }

  public static T GetPrevious<T>(this IReadOnlyList<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }

  public static T GetLast<T>(this IList<T> list) {
    return list[list.Count - 1];
  }

  public static void RemoveLast<T>(this IList<T> list) {
    if (!list.IsSet()) return;

    list.RemoveAt(list.Count - 1);
  }

  public static T GetRandom<T>(this IList<T> list) {
    return list[Random.Range(0, list.Count)];
  }

  /// <summary>
  /// Return a random element which is different than the given one (can be null).
  /// </summary>
  public static T GetRandomOtherThan<T>(this IList<T> list, T excludingElement) {
    if (excludingElement == null || list.Count == 1) return list.GetRandom();
    int excludingIndex = list.IndexOf(excludingElement);
    int randomIndex = excludingIndex;
    while (randomIndex == excludingIndex) {
      randomIndex = Random.Range(0, list.Count);
    }
    return list[randomIndex];
  }

  /// <summary>
  /// Return true if the element exists and not null.
  /// </summary>
  public static bool TryGetById<T>(this IList<UnityEngine.Object> objects, int id, out T element) where T : UnityEngine.Object {
    if (objects.Count > id) {
      element = objects[id] as T;
      return element ? true : false;
    } else {
      element = null;
      return false;
    }
  }

  /// <summary>
  /// Return true if the element exists in the array and not null.
  /// </summary>
  public static bool TryGetById<T>(this UnityEngine.Object[] objects, int id, out T element) where T : UnityEngine.Object {
    if (objects.Length > id) {
      element = objects[id] as T;
      return element ? true : false;
    } else {
      element = null;
      return false;
    }
  }

  /// <summary>
  /// Return true if the element exists in the array and not null.
  /// </summary>
  public static bool TryGetById(this UnityEngine.Object[] objects, int id, out UnityEngine.Object element) {
    if (objects.Length > id) {
      element = objects[id];
      return element ? true : false;
    } else {
      element = null;
      return false;
    }
  }

  // ? Move to GameObjectUtils
  /// <summary>
  /// Return the GameObject in the list which is nearest to the given position.
  /// </summary>
  // REFACTOR
  public static GameObject GetNearestTo(this IList<GameObject> list, Vector3 pos) {
    int nearestIndex = 0;
    float lastDist = Mathf.Infinity;

    for (int i = 0; i < list.Count; i++) {
      GameObject gameObject = list[i];
      float distanceToPos = Vector3.Distance(pos, gameObject.transform.position);
      if (distanceToPos < lastDist) {
        nearestIndex = i;
        lastDist = distanceToPos;
      }
    }

    return list[nearestIndex];
  }

  /// <summary>
  /// Return the GameObject in the list which is nearest to the given position.
  /// </summary>
  public static GameObject GetNearestTo(this IReadOnlyList<GameObject> list, Vector3 pos) {
    int nearestIndex = 0;
    float lastDist = Mathf.Infinity;

    for (int i = 0; i < list.Count; i++) {
      GameObject gameObject = list[i];
      float distanceToPos = Vector3.Distance(pos, gameObject.transform.position);
      if (distanceToPos < lastDist) {
        nearestIndex = i;
        lastDist = distanceToPos;
      }
    }

    return list[nearestIndex];
  }
  #endregion

  #region COLLECTION OPERATIONS
  private static System.Random rng = new System.Random();
  public static void Shuffle<T>(this IList<T> list) {
    Debug.Log("Shuffle");
    int n = list.Count;
    while (n > 1) {
      n--;
      int k = rng.Next(n + 1);
      T value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
  }

  // ! not pass by ref => not modify original list
  public static List<T> OrderByName<T>(this IList<T> list) where T : UnityEngine.Object {
    return list.OrderBy(item => item.name).ToList();
  }

  public static void RemoveNullEntries<T>(this IList<T> list) where T : class {
    for (int i = list.Count - 1; i >= 0; i--)
      if (Equals(list[i], null))
        list.RemoveAt(i);
  }

  public static void RemoveDefaultValues<T>(this IList<T> list) {
    for (int i = list.Count - 1; i >= 0; i--)
      if (Equals(default(T), list[i]))
        list.RemoveAt(i);
  }
  #endregion

  /// <summary>
  /// Destroy all GameObjects of the component list.
  /// </summary>
  public static void Destroy<T>(this T[] components) where T : MonoBehaviour {
    foreach (T component in components) {
#if UNITY_EDITOR
      if (!UnityEditor.EditorApplication.isPlaying) UnityEngine.Object.DestroyImmediate(component.gameObject);
#endif
      UnityEngine.Object.Destroy(component.gameObject);
    }
  }

  public static List<T> ToList<T>(this Array array) {
    var list = new List<T>();

    foreach (var item in array) {
      list.Add((T)item);
    }

    return list;
  }
}