using UnityEngine;

namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  ///   * The 'Product' base class (optional)
  ///   Products in different builders don’t have to belong to the same class hierarchy.
  /// </summary>
  public class House {
    public House(string name) => Container = new GameObject(name);

    public GameObject Container { get; }

    public void Add(GameObject part) {
      part.transform.SetParent(Container.transform);
    }
  }
}