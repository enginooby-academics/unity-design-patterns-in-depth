using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  public class ShapeTeleporter : IShapeMover {
    public void Move(GameObject shape, Vector3 endValue) => shape.transform.position = endValue;
  }
}