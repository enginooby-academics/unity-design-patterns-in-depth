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
      _currentShape.ProcessCalculation(_currentCalculator);
    }
  }
}