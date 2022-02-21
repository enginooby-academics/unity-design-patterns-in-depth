using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace VisitorPattern.Case1.Naive1 {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField] private ProceduralShape _currentShape;

    [Button]
    public void CalculateVolumeOfCurrentShape() {
      var result = _currentShape.CalculateVolume();
      print($"Volume of {_currentShape.name} is: {result}");
    }

    [Button]
    public void CalculateAreaOfCurrentShape() {
      var result = _currentShape.CalculateSurfaceArea();
      print($"Area of {_currentShape.name} is: {result}");
    }
  }
}