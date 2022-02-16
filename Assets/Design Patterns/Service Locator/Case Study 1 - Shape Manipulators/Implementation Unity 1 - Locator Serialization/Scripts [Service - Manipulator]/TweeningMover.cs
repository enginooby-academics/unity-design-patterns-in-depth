using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  public class TweeningMover : IShapeMover {
    public void Move(GameObject shape, Vector3 endValue) {
      // IMPL
      Debug.Log("Slowly move shape.");
    }
  }
}