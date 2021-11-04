using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CollectionUtils {
  public static bool IsUnset<T>(this List<T> list) {
    return list == null || list.Count == 0;
  }

  public static bool ValidateIndex<T>(this IList<T> list, int index) {
    return 0 <= index && index < list.Count;
  }

  // TODO: rename GetNext
  public static T NavNext<T>(this List<T> list, T currentItem) {
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

  public static T NavPrevious<T>(this List<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }

  public static T GetPrevious<T>(this IReadOnlyList<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int previous = currentIndex == 0 ? list.Count - 1 : currentIndex - 1;
    return list[previous];
  }


  public static T GetRandom<T>(this List<T> list) {
    return list[Random.Range(0, list.Count)];
  }

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

  public static T GetLast<T>(this List<T> list) {
    return list[list.Count - 1];
  }

  // ! not pass by ref => not modify original list
  public static List<GameObject> OrderByName(this List<GameObject> list) {
    list = list.OrderBy(item => item.name).ToList();
    return list;
  }

  /// <summary>
  /// Destroy all GameObjects of the component list.
  /// </summary>
  public static void Destroy<T>(this T[] components) where T : MonoBehaviour {
    foreach (T component in components) {
      Object.Destroy(component.gameObject);
    }
  }

  /// <summary>
  /// Return the GameObject in the list which is nearest to the given position.
  /// </summary>
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
}