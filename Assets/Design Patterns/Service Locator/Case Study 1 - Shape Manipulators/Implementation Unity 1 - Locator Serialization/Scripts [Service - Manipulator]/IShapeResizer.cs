using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  /// <summary>
  /// A 'service contract'
  /// </summary>
  public interface IShapeResizer {
    void Resize(GameObject shape, float endValue);
  }
}