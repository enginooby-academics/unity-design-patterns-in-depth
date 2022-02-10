#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace VisitorPattern.Case1.Naive2 {
  public class ClientCalculator : MonoBehaviour {
    [SerializeReference]
    private Calculator _currentCalculator;

    [SerializeField, InlineEditor]
    private ProceduralShape _currentShape;

    [Button]
    public void ProcessCalculationOnCurrentShape() {
      var result = _currentCalculator.Calculate(_currentShape);
      Debug.Log($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}