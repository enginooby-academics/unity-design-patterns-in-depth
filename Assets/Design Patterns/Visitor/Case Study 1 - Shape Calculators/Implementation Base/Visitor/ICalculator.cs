namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * The 'Visitor Contract'
  /// </summary>
  // ? Is interface needed when alread had abstract class Calculator
  // ? Add base generic interface for return type
  public interface ICalculator {
    /// <summary>
    /// * The 'Accept()' method
    /// </summary>
    float Calculate(ICalculatable shape);
  }
}
