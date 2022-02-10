#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace VisitorPattern.Case1.Base1 {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField, SerializeReference]
    private Calculator _currentCalculator;

    [SerializeField, InlineEditor]
    private ProceduralShape _currentShape;

    [Button]
    [ContextMenu(nameof(ProcessCalculationOnCurrentShape))]
    public void ProcessCalculationOnCurrentShape() {
      var result = _currentShape.ProcessCalculation(_currentCalculator);
      print($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}