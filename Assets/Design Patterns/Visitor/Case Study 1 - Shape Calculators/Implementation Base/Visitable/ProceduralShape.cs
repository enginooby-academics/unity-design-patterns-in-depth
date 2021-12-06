using Sirenix.OdinInspector;
using UnityEngine;

namespace VisitorPattern.Case1.Base {
  public abstract class ProceduralShape : MonoBehaviour, ICalculatable {
    public float ProcessCalculation(ICalculator calculator) => calculator.Calculate(this);

    [Button]
    public void Calculate() {
      ICalculator calculator = CalculatorManager.Instance.CurrentCalculator;
      float result = ProcessCalculation(calculator);
      print($"{calculator.GetType().Name}: {result}");
    }
  }
}
