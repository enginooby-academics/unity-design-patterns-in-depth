namespace VisitorPattern.Case1.Base1 {
  /// <summary>
  ///   * [The 'Visitable Element Contract']
  /// </summary>
  public interface ICalculatable {
    /// <summary>
    ///   * [The 'Accept()' method]
    /// </summary>
    double ProcessCalculation(Calculator calculator);
  }
}