namespace VisitorPattern.Case1.Base2 {
  /// <summary>
  /// * The 'Visitable Element' interface
  /// </summary>
  public interface ICalculatable {
    /// <summary>
    /// * The 'Accept' method
    /// </summary>
    double ProcessCalculation(Calculator calculator);
  }
}
