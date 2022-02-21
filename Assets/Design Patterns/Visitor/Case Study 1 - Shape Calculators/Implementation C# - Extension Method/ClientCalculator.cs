using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace VisitorPattern.Case1.CSharp {
  public class ClientCalculator : MonoBehaviour {
    [SerializeField] private ProceduralShape _currentShape;

    [SerializeField] [SerializeReference] private Calculator _currentCalculator;

    [Button]
    public void ProcessCalculationOnCurrentShape() {
      var result = _currentShape.ProcessCalculation(_currentCalculator);
      Debug.Log($"{_currentCalculator.GetType().Name}: {result}");
    }
  }
}