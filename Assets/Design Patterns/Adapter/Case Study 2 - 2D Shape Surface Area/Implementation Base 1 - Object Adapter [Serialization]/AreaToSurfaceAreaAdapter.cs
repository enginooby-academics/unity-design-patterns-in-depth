using UnityEngine;

namespace AdapterPattern.Case2.Base1 {
  /// <summary>
  /// * [The 'Adapter' base class]
  /// </summary>
  public abstract class AreaToSurfaceAreaAdapter : MonoBehaviour, ISurfaceArea {
    [SerializeField]
    protected IAreaContainer _shape2d;

    public abstract double GetSurfaceArea();
  }
}