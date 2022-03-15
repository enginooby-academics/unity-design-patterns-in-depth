using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Enginooby.Utils {
  public static class CollectionUtils {
    public static List<T> ToList<T>(this Array array) => array.Cast<T>().ToList();


    // =================================================================================================================

    #region VALIDATION

    public static bool HasIndex<T>(this IEnumerable<T> collection, int index) =>
      index.IsInRange(0, collection.Count() - 1);

    /// <summary>
    ///   Does collection has at least one element of the given value?
    ///   <example>(1, 2, 3, 1).Has(4) -> false</example>
    /// </summary>
    public static bool Has<T>(this IEnumerable<T> collection, T value) where T : IComparable {
      return collection.Any(element => element.CompareTo(value) == 0);
    }

    public static bool IsNullOrEmpty<T>([CanBeNull] this IEnumerable<T> collection) =>
      collection == null || !collection.Any();

    #endregion

    // =================================================================================================================

    #region RETRIEVAL

    /// <summary>
    ///   Return -1 if the given element doesn't exist in the collection.
    /// </summary>
    public static int IndexOf<T>(this IEnumerable<T> collection, T elementToFind) {
      var i = 0;
      foreach (var element in collection) {
        if (Equals(element, elementToFind))
          return i;
        i++;
      }

      return -1;
    }

    // ? Rename: Next
    public static T GetNext<T>(this IEnumerable<T> collection, T currentItem) {
      var currentIndex = collection.IndexOf(currentItem);
      var nextIndex = currentIndex == collection.Count() - 1 ? 0 : currentIndex + 1;
      return collection.ElementAt(nextIndex);
    }

    public static T GetPrevious<T>(this IEnumerable<T> list, T currentItem) {
      var currentIndex = list.IndexOf(currentItem);
      var previousIndex = currentIndex == 0 ? list.Count() - 1 : currentIndex - 1;
      return list.ElementAt(previousIndex);
    }

    public static T GetLast<T>(this IEnumerable<T> collection) => collection.ElementAt(collection.Count() - 1);

    public static T GetRandom<T>(this IEnumerable<T> list) => list.ElementAt(Random.Range(0, list.Count()));

    // ? Rename: GetRandomExcluding(params...)
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

      element = default;
      return false;
    }

    private static readonly Dictionary<Type, object> NullKeys = new();

    private static T GetNullKey<T>() => NullKeys.TryGetValue(typeof(T), out var nullKey) ? (T) nullKey : default;

    // ? Naming convention: _OrDefault
    public static TValue GetValueOrDefault<TKey, TValue>(
      this Dictionary<TKey, TValue> dictionary,
      TKey key,
      TValue defaultValue = default) {
      if (dictionary is null) return defaultValue;

      key ??= GetNullKey<TKey>();
      return key is not null && dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    #endregion

    // =================================================================================================================

    #region MODIFICATION

    private static readonly System.Random rng = new();

    public static void Shuffle<T>(this IList<T> list) {
      var n = list.Count;
      while (n > 1) {
        n--;
        var k = rng.Next(n + 1);
        (list[k], list[n]) = (list[n], list[k]);
      }
    }

    /// <summary>
    ///   Remove all the elements whose values equal to the given value. <br />
    ///   <example> (1, 2, 3, 1).RemoveEntriesOfValue(1) -> (2, 3) </example>
    /// </summary>
    public static void RemoveEntriesOfValue<T>(this IList<T> list, [CanBeNull] T entryValue) {
      for (var i = list.Count - 1; i >= 0; i--)
        if (Equals(list[i], entryValue))
          list.RemoveAt(i);
    }

    public static void RemoveNullEntries<T>(this IList<T> list) where T : class => list.RemoveEntriesOfValue(null);

    public static void RemoveDefaultValues<T>(this IList<T> list) => list.RemoveEntriesOfValue(default);

    // https://stackoverflow.com/questions/1211608/possible-to-iterate-backwards-through-a-foreach
    public static IEnumerable<T> FastReverse<T>(this IList<T> list) {
      for (var i = list.Count - 1; i >= 0; i--) yield return list[i];
    }

    public static void RemoveLast<T>(this IList<T> list) {
      if (list.IsNullOrEmpty()) return;

      list.RemoveAt(list.Count - 1);
    }

    /// <summary>
    ///   Return true if not contains.
    /// </summary>
    public static bool AddIfNotContains<T>(this IList<T> list, T element) {
      if (list.Contains(element)) return false;

      list.Add(element);
      return true;
    }

    // FIX: Not working
    public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, IEnumerable<T> elements) {
      foreach (var element in elements) collection.Append(element);

      return collection;
    }

    /// <summary>
    ///   Perform an action on each item.
    /// </summary>
    public static IEnumerable<T> ForEach<T>([CanBeNull] this IEnumerable<T> collection, Action<T> action) {
      foreach (var obj in collection)
        action?.Invoke(obj);
      return collection;
    }

    #endregion
  }
}