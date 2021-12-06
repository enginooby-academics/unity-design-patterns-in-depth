using UnityEngine;

namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// Manage and switch between different visitors.
  /// </summary>
  public class CalculatorManager : MonoBehaviourSingleton<CalculatorManager> {
    [SerializeField, SerializeReference]
    private ICalculator _currentCalculator;
    public ICalculator CurrentCalculator => _currentCalculator;
  }
}