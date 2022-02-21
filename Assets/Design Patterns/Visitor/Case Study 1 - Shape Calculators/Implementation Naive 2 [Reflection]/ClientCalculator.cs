using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace VisitorPattern.Case1.Naive2 {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField] private ProceduralShape _currentShape;

    [SerializeReference] private Calculator _currentCalculator;

    [Button]
    public void ProcessCalculationOnCurrentShape() {
      var result = _currentCalculator.Calculate(_currentShape);
      Debug.Log($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}