using System.Collections.Generic;
using UnityEngine;

public static class CollectionUtils {
  public static bool IsUnset<T>(this List<T> list) {
    return list == null || list.Count == 0;
  }

  public static bool ValidateIndex<T>(this IList<T> list, int index) {
    return 0 <= index && index < list.Count;
  }

  public static T NavNext<T>(this List<T> list, T currentItem) {
    int currentIndex = list.IndexOf(currentItem);
    int nextIndex = currentIndex == list.Count - 1 ? 0 : currentIndex + 1;
    return list[nextIndex];
  }

  public static T NavPrevious<T>(this List<T> list, T currentItem) {
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

  public static void Destroy<T>(this T[] components) where T : MonoBehaviour {
    foreach (T component in components) {
      Object.Destroy(component.gameObject);
    }
  }
}