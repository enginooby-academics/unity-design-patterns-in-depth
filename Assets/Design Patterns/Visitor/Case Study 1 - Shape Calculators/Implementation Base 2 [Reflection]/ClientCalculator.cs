using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace VisitorPattern.Case1.Base2 {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField] [InlineEditor] private ProceduralShape _currentShape;

    [SerializeField] [SerializeReference] private Calculator _currentCalculator;

    [Button]
    public void ProcessCalculationOnCurrentShape() {
      var result = _currentShape.ProcessCalculation(_currentCalculator);
      print($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}