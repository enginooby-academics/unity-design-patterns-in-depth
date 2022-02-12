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

      return (double) calculatingMethod?.Invoke(this, new[] {shape});

      throw new NotImplementedException();
    }

    protected abstract double Calculate(ProceduralCube shape);
    protected abstract double Calculate(ProceduralSphere shape);

    // ! If visitor's operation is not mandatory for all elements, declare virtual methods instead:
    // private SystemException GetNotImplementedException(ICalculatable element) {
    //   return new NotImplementedException($"{this.GetType().Name} does not have implementation for {element.GetType().Name} type.");
    // }

    // /// <summary>
    // /// Throw NotImplementedException by default, hence don't invoke base if override.
    // /// </summary>
    // protected virtual float Calculate(ProceduralCube shape) {
    //   throw GetNotImplementedException(shape);
    // }

    // /// <summary>
    // /// Throw NotImplementedException by default, hence don't invoke base if override.
    // /// </summary>
    // protected virtual float Calculate(ProceduralSphere shape) {
    //   throw GetNotImplementedException(shape);
    // }
  }
}