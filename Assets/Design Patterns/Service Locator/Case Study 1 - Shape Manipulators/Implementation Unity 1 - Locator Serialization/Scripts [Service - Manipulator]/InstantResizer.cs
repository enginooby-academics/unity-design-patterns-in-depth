using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  /// <summary>
  /// * A concrete service class.
  /// Resize shape instantly.
  /// </summary>
  public class InstantResizer : IShapeResizer {
    public void Resize(GameObject shape, float endValue) => shape.transform.localScale = Vector3.one * endValue;
  }
}