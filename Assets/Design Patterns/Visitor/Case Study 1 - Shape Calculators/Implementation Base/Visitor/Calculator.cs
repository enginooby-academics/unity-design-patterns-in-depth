using System;
using System.Reflection;

namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * The 'Abstract Visitor' class
  /// </summary>
  public abstract class Calculator : ICalculator {
    public float Calculate(ICalculatable element) {
      Type elementType = element.GetType();
      Type calculatorType = this.GetType();
      // UTIL: get non public method using reflection
      var calculationMethod = calculatorType.GetMethod(nameof(Calculate), BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { elementType }, null);

      // if method Calculation(concrete shapeType) exists in this Calculator, which is not Calculate(ProceduralShape)
      if (calculationMethod != null && calculationMethod.GetParameters()[0].ParameterType != typeof(ICalculatable)) {
        return (float)calculationMethod.Invoke(this, new[] { element });
      }

      throw GetNotImplementedException(element);
    }

    private SystemException GetNotImplementedException(ICalculatable element) {
      return new NotImplementedException($"{this.GetType().Name} does not have implementation for {element.GetType().Name} type.");
    }

    // ! If visitor's operation is mandatory for all elements, declare abstract methods instead

    /// <summary>
    /// Throw NotImplementedException by default, hence don't invoke base if override.
    /// </summary>
    protected virtual float Calculate(ProceduralCube shape) {
      throw GetNotImplementedException(shape);
    }

    /// <summary>
    /// Throw NotImplementedException by default, hence don't invoke base if override.
    /// </summary>
    protected virtual float Calculate(ProceduralSphere shape) {
      throw GetNotImplementedException(shape);
    }

    /// <summary>
    /// Throw NotImplementedException by default, hence don't invoke base if override.
    /// </summary>
    protected virtual float Calculate(ProceduralCylinder shape) {
      throw GetNotImplementedException(shape);
    }
  }
}
