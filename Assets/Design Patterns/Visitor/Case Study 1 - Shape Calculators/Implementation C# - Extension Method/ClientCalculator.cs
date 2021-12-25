using Sirenix.OdinInspector;
using UnityEngine;

namespace VisitorPattern.Case1.CSharp {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField, SerializeReference]
    private Calculator _currentCalculator;

    [SerializeField, InlineEditor]
    private ProceduralShape _currentShape;

    [Button]
    public void ProcessCalculationOnCurrentShape() {
      var result = _currentShape.ProcessCalculation(_currentCalculator);
      Debug.Log($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}