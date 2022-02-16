using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  /// <summary>
  /// * A concrete service class.
  /// Interpolated shape resizing for a time period.
  /// </summary>
  public class TweeningResizer : IShapeResizer {
    public void Resize(GameObject shape, float endValue) {
      // IMPL
      Debug.Log("Slowly resize shape");
    }
  }
}