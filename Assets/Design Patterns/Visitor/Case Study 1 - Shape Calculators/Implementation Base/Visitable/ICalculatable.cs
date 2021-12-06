namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * The 'Visitable Element Contract'
  /// </summary>
  public interface ICalculatable {
    float ProcessCalculation(ICalculator calculator);
  }
}
