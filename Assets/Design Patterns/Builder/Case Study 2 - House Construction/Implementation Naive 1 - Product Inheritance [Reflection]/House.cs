using System.Collections;
using UnityEngine;

namespace BuilderPattern.Case2.Naive1 {
  public abstract class House {
    public GameObject Container { get; private set; }

    public House(string name) {
      Container = new GameObject(name);
    }

    public void Add(GameObject part) {
      part.transform.SetParent(Container.transform);
    }

    // ! Contract building methods
    public abstract IEnumerator BuildBase(float speed);
    public abstract IEnumerator BuildRoof(float speed);
    public abstract IEnumerator BuildDoor(float speed);
    public abstract IEnumerator BuildWindows(float speed);
    public abstract IEnumerator BuildChimney(float speed);
  }
}