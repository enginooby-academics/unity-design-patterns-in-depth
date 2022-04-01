using System;

namespace VisitorPattern.Case1.CSharp {
  /// <summary>
  ///   * [The 'Abstract Visitor' class]
  /// </summary>
  public abstract class Calculator {
    public double Calculate(ProceduralShape shape) {
      var shapeType = shape.GetType();
      var calculatorType = GetType();
      var calculatingMethod = calculatorType.GetNonPublicMethod(nameof(Calculate), shapeType);
      var result = (double) calculatingMethod?.Invoke(this, new object[] {shape})!;

      return result;
    }

    protected abstract double Calculate(ProceduralCube shape);
    protected abstract double Calculate(ProceduralSphere shape);
  }
}