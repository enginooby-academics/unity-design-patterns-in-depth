using UnityEngine;

namespace AdapterPattern.Case2.Base2 {
  /// <summary>
  /// * [The 'Adapter' base class]
  /// </summary>
  public abstract class AreaToSurfaceAreaAdapter : MonoBehaviour, ISurfaceArea {
    protected readonly IArea _shape2d;

    public AreaToSurfaceAreaAdapter(IArea shape2d) {
      _shape2d = shape2d;
    }

    public abstract double GetSurfaceArea();
  }
}