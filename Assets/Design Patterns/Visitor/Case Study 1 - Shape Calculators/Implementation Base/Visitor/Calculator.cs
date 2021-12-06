using System;

namespace VisitorPattern.Case1.Base {
  // ? How to force derived classes only overloading Calculate() (declare abstract methods in base?)
  /// <summary>
  /// * The 'Abstract Visitor' class
  /// </summary>
  public abstract class Calculator : ICalculator {
    public float Calculate(ICalculatable element) {
      Type elementType = element.GetType();
      Type calculatorType = this.GetType();
      var calculationMethod = calculatorType.GetMethod(nameof(Calculate), new Type[] { elementType });

      // if method Calculation(concrete shapeType) exists in this Calculator, which is not Calculate(ProceduralShape)
      if (calculationMethod != null && calculationMethod.GetParameters()[0].ParameterType != typeof(ICalculatable)) {
        return (float)calculationMethod.Invoke(this, new[] { element });
      }

      throw new InvalidOperationException($"{calculatorType.Name} does not have implementation for {elementType.Name} type.");
    }
  }
}
