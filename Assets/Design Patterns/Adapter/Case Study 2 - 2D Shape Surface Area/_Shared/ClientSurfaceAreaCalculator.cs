using Sirenix.OdinInspector;
using UnityEngine;

namespace AdapterPattern.Case2 {
  public abstract class ClientSurfaceAreaCalculator : MonoBehaviour {
    [Button]
    public abstract void CalculateSurfaceArea();
  }
}