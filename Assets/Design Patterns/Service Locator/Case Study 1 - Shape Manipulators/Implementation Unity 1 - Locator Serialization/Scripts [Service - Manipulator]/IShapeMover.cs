using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  public interface IShapeMover {
    void Move(GameObject shape, Vector3 endValue);
  }
}