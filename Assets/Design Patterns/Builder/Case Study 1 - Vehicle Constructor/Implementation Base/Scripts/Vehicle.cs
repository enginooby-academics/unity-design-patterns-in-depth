using System.Collections.Generic;
using UnityEngine;

namespace BuilderPattern.Case1.Base {
  // Our final product
  public class Vehicle {
    private readonly string _name;
    private readonly List<GameObject> _parts = new List<GameObject>();

    // Constructor method
    public Vehicle(string name) {
      _name = name;
      parent = new GameObject(name);
    }

    public GameObject parent { get; }

    public void AddPart(GameObject part, Vector3 localPosition) {
      part.transform.SetParent(parent.transform);
      part.transform.localPosition = localPosition;
      _parts.Add(part);
    }

    public string GetPartsList() {
      var partsList = _name + " parts:\n\t";
      foreach (var part in _parts) partsList += part.name + " ";

      return partsList;
    }

    // Provides a common function to make the parts. Not truly a part of the standard
    // pattern, but included in this example to make part creation easier.
    public GameObject MakePart(PrimitiveType primitiveType, string name, Vector3 scale, Color color) {
      var go = GameObject.CreatePrimitive(primitiveType);
      go.name = name;
      go.transform.localScale = scale;
      go.SetMaterialColor(color);
      return go;
    }
  }
}