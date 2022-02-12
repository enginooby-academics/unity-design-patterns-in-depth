namespace VisitorPattern.Case1.Base1 {
  /// <summary>
  ///   * [The 'Abstract Visitor' class]
  /// </summary>
  public abstract class Calculator {
    public abstract double Calculate(ProceduralCube shape);
    public abstract double Calculate(ProceduralSphere shape);
  }
}