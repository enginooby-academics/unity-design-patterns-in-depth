using Sirenix.OdinInspector;
using UnityEngine;

namespace VisitorPattern.Case1.Base {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField, SerializeReference]
    private Calculator _currentCalculator;

    [SerializeField, InlineEditor]
    private ProceduralShape _currentShape;

    [Button]
    public void ProcessCalculationOnCurrentShape() {
      float result = _currentShape.ProcessCalculation(_currentCalculator);
      print($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}