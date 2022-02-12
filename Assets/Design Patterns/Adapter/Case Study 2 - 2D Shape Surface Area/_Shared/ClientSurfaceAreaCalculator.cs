using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace AdapterPattern.Case2 {
  public abstract class ClientSurfaceAreaCalculator : MonoBehaviour {
    [Button]
    public abstract void CalculateSurfaceArea();
  }
}