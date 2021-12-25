using Sirenix.OdinInspector;
using UnityEngine;

namespace VisitorPattern.Case1.Naive {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField, InlineEditor]
    private ProceduralShape _currentShape;

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