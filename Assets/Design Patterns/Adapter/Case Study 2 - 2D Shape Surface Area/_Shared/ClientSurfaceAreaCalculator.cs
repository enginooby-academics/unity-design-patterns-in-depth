using Sirenix.OdinInspector;
using UnityEngine;

namespace AdapterPattern.Case2 {
  public abstract class ClientSurfaceAreaCalculator : MonoBehaviour {
    [SerializeField]
    protected GameObject _shape;

    [Button]
    public abstract void CalculateSurfaceArea();
  }
}