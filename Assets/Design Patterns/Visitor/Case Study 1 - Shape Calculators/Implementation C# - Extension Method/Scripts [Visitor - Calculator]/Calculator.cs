using System;

namespace VisitorPattern.Case1.CSharp {
  /// <summary>
  /// * [The 'Abstract Visitor' class]
  /// </summary>
  public abstract class Calculator {
    public float Calculate(ProceduralShape shape) {
      var shapeType = shape.GetType();
      var calculatorType = this.GetType();
      var calculatingMethod = calculatorType.GetNonPublicMethod(nameof(Calculate), paramType: shapeType);

      return (float)calculatingMethod?.Invoke(this, new[] { shape });

      throw new NotImplementedException();
    }

    protected abstract float Calculate(ProceduralCube shape);
    protected abstract float Calculate(ProceduralSphere shape);

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
