using System;

namespace VisitorPattern.Case1.Naive2 {
  public abstract class Calculator {
    public double Calculate(ProceduralShape shape) {
      var shapeType = shape.GetType();
      var calculatorType = this.GetType();
      var calculatingMethod = calculatorType.GetNonPublicMethod(nameof(Calculate), paramType: shapeType);

      return (double)calculatingMethod?.Invoke(this, new[] { shape });

      throw new NotImplementedException();
    }

    protected abstract double Calculate(ProceduralCube shape);
    protected abstract double Calculate(ProceduralSphere shape);
  }
}
