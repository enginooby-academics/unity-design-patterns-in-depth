namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * [The 'Visitable Element Contract']
  /// </summary>
  public interface ICalculatable {
    /// <summary>
    /// * [The 'Accept()' method]
    /// </summary>
    float ProcessCalculation(Calculator calculator);
  }
}
