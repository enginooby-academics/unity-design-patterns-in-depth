using System.Collections.Generic;
using UnityEngine;

namespace BuilderPattern.Case2.Base1 {
  /// <summary>
  /// * The 'Product' base class
  /// Optional: products in different builders don’t have to belong to the same class hierarchy.  
  /// </summary>
  public class House {
    public GameObject Container { get; private set; }

    private string _name;
    private List<GameObject> _parts = new List<GameObject>();

    public House(string name) {
      Container = new GameObject(name);
    }

    public GameObject MakePart(string name) {
      GameObject go = new GameObject(name);
      return go;
    }

    public void Add(GameObject part) {
      part.transform.SetParent(Container.transform);
    }
  }
}